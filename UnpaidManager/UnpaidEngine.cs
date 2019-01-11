using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager
{
    public class UnpaidEngine: IUnpaidEngineHandler
    {
        private readonly IUnpaidClient _unpaidClient;
        private readonly IUnpaidRequestClient _unpaidRequestClient;
        private readonly INotification _notification;
        private readonly IUnpaidResponseClient _unpaidResponseClient;

        public UnpaidEngine(IUnpaidClient unpaidClient, IUnpaidRequestClient unpaidRequestClient, INotification notification, IUnpaidResponseClient unpaidResponseClient)
        {
            _unpaidClient = unpaidClient;
            _unpaidRequestClient = unpaidRequestClient;
            _notification = notification;
            _unpaidResponseClient = unpaidResponseClient;
        }

        public async Task<IEnumerable<UnpaidOutput>> HandleUnpaidAsync(IEnumerable<UnpaidInput> unpaids, string idempotencyKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                // Log Error.
                return null;
            }

            if (unpaids == null)
            {
                // Log Error.
                return null;
            }

            var unpaidList = unpaids.ToList();
            if (!unpaidList.Any())
            {
                // Log Error.
                return null;
            }

            // First add the unpaids to the storage.
            var unpaidResult = await _unpaidClient.AddUnpaidAsync(unpaidList, idempotencyKey, cancellationToken);
            if (unpaidResult <= 0)
            {
                // Log Error.
                return null;
            }

            // Get unpaids to work with by idempotencyKey
            var unpaidsByIdempotencyResult = await _unpaidClient.GetUnpaidsByIdempotencyKeyAsync(idempotencyKey, cancellationToken);
            if (unpaidsByIdempotencyResult == null)
            {
                // Log Error.
                return null;
            }

            var byIdempotencyResultList = unpaidsByIdempotencyResult.ToList();

            if (!byIdempotencyResultList.Any())
            {
                // Log Error.
                return null;
            }

            // Add UnpaidRequest to storage and default status as pending.
            var unpaidRequestResult = await _unpaidRequestClient.AddUnpaidRequestAsync(byIdempotencyResultList, Notification.Push, Status.Pending, cancellationToken);

            if (unpaidRequestResult <= 0)
            {
                // Log Error.
                return null;
            }

            return await HandleUnpaidRequestAsync(byIdempotencyResultList, cancellationToken);
        }

        public async Task<IEnumerable<UnpaidOutput>> HandleUnpaidRequestAsync(IEnumerable<TbUnpaid> unpaids, CancellationToken cancellationToken)
        {
            var unpaidOutputList = new List<UnpaidOutput>();

            foreach (var unpaid in unpaids)
            {
                var unpaidOutput = new UnpaidOutput
                {
                    PolicyNumber = unpaid.PolicyNumber,
                    IdNumber = unpaid.IdNumber,
                    Name = unpaid.Name,
                    Message = unpaid.Message,
                    Status = Status.Pending.ToString(),
                    ErrorMessage = string.Empty
                };

                // create notification task.
                var notificationTask = _notification.SendAsync($"Dear {unpaid.Name}", unpaid.Message, unpaid.IdNumber, cancellationToken);

                // Get all UnpaidRequests by idempotencyKey
                var unpaidRequestsToUpdate = await _unpaidRequestClient.GetAllUnpaidRequestAsync(unpaid.UnpaidId, cancellationToken);

                if (unpaidRequestsToUpdate == null)
                {
                    // Log Warning. No UnpaidRequests to update.
                    notificationTask.Dispose();
                    unpaidOutputList.Add(unpaidOutput);
                    continue;
                }

                var singleUnpaidRequestToUpdate = unpaidRequestsToUpdate.FirstOrDefault();

                // Update UnpaidRequest status.
                if (singleUnpaidRequestToUpdate != null)
                {
                    // evaluate notification task result.
                    var notificationResult = await notificationTask;

                    var status = Status.Failed;

                    if (notificationResult.StatusCode == HttpStatusCode.Accepted)
                    {
                        status = Status.Success;
                    }

                    var updateUnpaidRequestResult = await _unpaidRequestClient.UpdateUnpaidRequestAsync(singleUnpaidRequestToUpdate.UnpaidRequestId, Notification.Push, status, notificationResult.AdditionalErrorMessage, cancellationToken);
                    if (updateUnpaidRequestResult <= 0)
                    {
                        // Log Error. Update failed.
                        return null;
                    }

                    unpaidOutput.Status = status.ToString();
                    unpaidOutput.ErrorMessage = notificationResult.AdditionalErrorMessage;
                }

                unpaidOutputList.Add(unpaidOutput);
            }

            return unpaidOutputList;
        }

        public async Task<IEnumerable<UnpaidResponseOutput>> HandleUnpaidResponseAsync(IEnumerable<UnpaidResponseInput> unpaidResponseInputs, CancellationToken cancellationToken)
        {
            if (unpaidResponseInputs == null)
            {
                // Log Error.
                return null;
            }

            var responseInputList = unpaidResponseInputs.ToList();

            if (!responseInputList.Any())
            {
                // Log Error.
                return null;
            }

            var unpaidResponseOutputList = new List<UnpaidResponseOutput>();

            foreach (var unpaidResponseInput in responseInputList)
            {
                var unpaidResonseOutput = new UnpaidResponseOutput
                {
                    PolicyNumber = unpaidResponseInput.PolicyNumber,
                    IdNumber = unpaidResponseInput.IdNumber,
                    Accepted = unpaidResponseInput.Accepted,
                    ContactOption = unpaidResponseInput.ContactOption,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    ErrorMessage = "Notification not found."
                };

                var unpaidRequest = await _unpaidRequestClient.GetLatestSuccessfulUnpaidRequestAsync(unpaidResponseInput, cancellationToken);
                if (unpaidRequest == null)
                {
                    // Log Error.
                    unpaidResponseOutputList.Add(unpaidResonseOutput);
                    continue;
                }

                // Check if already exists.
                var unpaidResponse = await _unpaidResponseClient.GetUnpaidResponseAsync(unpaidRequest.UnpaidRequestId, cancellationToken);
                if (unpaidResponse != null && unpaidResponse.Any())
                {
                    // Log Warning.
                    unpaidResonseOutput.HttpStatusCode = HttpStatusCode.AlreadyReported;
                    unpaidResonseOutput.ErrorMessage = "Notification response already exists.";
                    unpaidResponseOutputList.Add(unpaidResonseOutput);
                    continue;
                }

                var addUnpaidResponseResult = await _unpaidResponseClient.AddPendingUnpaidResponseAsync(unpaidResponseInput, unpaidRequest.UnpaidRequestId, cancellationToken);

                if (addUnpaidResponseResult <= 0)
                {
                    // Log Error.
                    unpaidResonseOutput.HttpStatusCode = HttpStatusCode.InternalServerError;
                    unpaidResonseOutput.ErrorMessage = "Error adding notification response.";
                    unpaidResponseOutputList.Add(unpaidResonseOutput);
                    continue;
                }

                unpaidResonseOutput.HttpStatusCode = HttpStatusCode.Accepted;
                unpaidResonseOutput.ErrorMessage = string.Empty;
                unpaidResponseOutputList.Add(unpaidResonseOutput);
            }

            return unpaidResponseOutputList;
        }
    }
}
