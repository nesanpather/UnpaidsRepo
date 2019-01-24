using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using Hangfire;
using Microsoft.Extensions.Logging;
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
        private readonly IUnpaidNotificationApiClient _unpaidNotificationApiClient;
        private readonly IUnpaidBatchClient _unpaidBatchClient;
        private readonly ILogger<UnpaidEngine> _logger;

        public UnpaidEngine(IUnpaidClient unpaidClient, IUnpaidRequestClient unpaidRequestClient, INotification notification, IUnpaidResponseClient unpaidResponseClient, IUnpaidNotificationApiClient unpaidNotificationApiClient, IUnpaidBatchClient unpaidBatchClient, ILogger<UnpaidEngine> logger)
        {
            _unpaidClient = unpaidClient;
            _unpaidRequestClient = unpaidRequestClient;
            _notification = notification;
            _unpaidResponseClient = unpaidResponseClient;
            _unpaidNotificationApiClient = unpaidNotificationApiClient;
            _unpaidBatchClient = unpaidBatchClient;
            _logger = logger;
        }

        public async Task<UnpaidOutput> HandleUnpaidAsync(IEnumerable<UnpaidInput> unpaids, string idempotencyKey, string userName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidEngine.HandleUnpaidAsync - idempotencyKey is null or empty");
                return null;
            }

            if (unpaids == null)
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidEngine.HandleUnpaidAsync - unpaids is null");
                return null;
            }

            var unpaidList = unpaids.ToList();
            if (!unpaidList.Any())
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidEngine.HandleUnpaidAsync - unpaids is empty");
                return null;
            }

            // First add create new unpaid batch.
            var unpaidBatchResult = await _unpaidBatchClient.AddUnpaidBatchAsync(idempotencyKey, Status.Pending, userName, cancellationToken);
            if (unpaidBatchResult <= 0)
            {
                _logger.LogError((int)LoggingEvents.InsertItem, "UnpaidEngine.HandleUnpaidAsync - _unpaidBatchClient.AddUnpaidBatchAsync returned no rows", new { BatchKey = idempotencyKey });
                return null;
            }

            // Get the batchId that was just added.
            var getUnpaidBatchResult = await _unpaidBatchClient.GetUnpaidBatchByBatchKeyAsync(idempotencyKey, cancellationToken);
            if (getUnpaidBatchResult == null)
            {
                _logger.LogError((int)LoggingEvents.GetItem, "UnpaidEngine.HandleUnpaidAsync - _unpaidBatchClient.GetUnpaidBatchByBatchKeyAsync returned null", new { BatchKey = idempotencyKey });
                return null;
            }

            var singleBatchToUpdate = getUnpaidBatchResult.FirstOrDefault();
            if (singleBatchToUpdate == null)
            {
                return new UnpaidOutput
                {
                    Status = Status.Failed.ToString(),
                    ErrorMessage = string.Empty
                };
            }
           
            var unpaidResult = await _unpaidClient.AddUnpaidAsync(unpaidList, singleBatchToUpdate.UnpaidBatchId, cancellationToken);
            if (unpaidResult <= 0)
            {
                _logger.LogError((int)LoggingEvents.InsertItem, "UnpaidEngine.HandleUnpaidAsync - _unpaidClient.AddUnpaidAsync returned no rows", new { BatchId = singleBatchToUpdate.UnpaidBatchId });
                return null;
            }

            // Get unpaids to work with by idempotencyKey
            var unpaidsByIdempotencyResult = await _unpaidClient.GetUnpaidsByIdempotencyKeyAsync(idempotencyKey, cancellationToken);
            if (unpaidsByIdempotencyResult == null)
            {
                _logger.LogError((int)LoggingEvents.GetItem, "UnpaidEngine.HandleUnpaidAsync - _unpaidClient.GetUnpaidsByIdempotencyKeyAsync returned null", new { BatchKey = idempotencyKey });
                return null;
            }

            var byIdempotencyResultList = unpaidsByIdempotencyResult.ToList();

            if (!byIdempotencyResultList.Any())
            {
                _logger.LogError((int)LoggingEvents.GetItem, "UnpaidEngine.HandleUnpaidAsync - _unpaidClient.GetUnpaidsByIdempotencyKeyAsync returned no rows", new { BatchKey = idempotencyKey });
                return null;
            }

            // Add UnpaidRequest to storage and default status as pending.
            var unpaidRequestResult = await _unpaidRequestClient.AddUnpaidRequestAsync(byIdempotencyResultList, Notification.Push, Status.Pending, cancellationToken);
            if (unpaidRequestResult <= 0)
            {
                _logger.LogError((int)LoggingEvents.InsertItem, "UnpaidEngine.HandleUnpaidAsync - _unpaidRequestClient.AddUnpaidRequestAsync returned no rows", new { BatchKey = idempotencyKey });
                return null;
            }

            var delegateToNotificationApiResult = await _unpaidNotificationApiClient.DelegateToNotificationApiAsync(idempotencyKey, cancellationToken);

            return new UnpaidOutput
            {
                Status = Status.Pending.ToString(),
                ErrorMessage = string.Empty
            };
        }

        // Hangfire processes this method.
        [AutomaticRetry(Attempts = 3)]
        public async Task<bool> HandleUnpaidRequestAsync(IEnumerable<TbUnpaid> unpaids, string idempotencyKey, CancellationToken cancellationToken)
        {
            if (unpaids == null)
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidEngine.HandleUnpaidRequestAsync - unpaids is null");
                return false;
            }

            var isBatchSuccessful = false;

            foreach (var unpaid in unpaids)
            {
                // Get all pending UnpaidRequests by idempotencyKey
                var unpaidRequestsToUpdate = await _unpaidRequestClient.GetAllUnpaidRequestAsync(unpaid.UnpaidId, Status.Pending, cancellationToken);

                if (unpaidRequestsToUpdate == null)
                {
                    _logger.LogWarning((int)LoggingEvents.GetItem, "UnpaidEngine.HandleUnpaidRequestAsync - _unpaidRequestClient.GetAllUnpaidRequestAsync returned null", new { BatchKey = idempotencyKey });
                    isBatchSuccessful = false;
                    continue;
                }

                var singleUnpaidRequestToUpdate = unpaidRequestsToUpdate.FirstOrDefault();
                
                if (singleUnpaidRequestToUpdate == null) continue;

                var correlationId = $"{idempotencyKey}_{singleUnpaidRequestToUpdate.UnpaidRequestId}";

                // Send notification.
                var notificationResult = await _notification.SendAsync($"Dear {unpaid.Name}", unpaid.Message, unpaid.IdNumber, correlationId, cancellationToken); ;

                var status = Status.Failed;

                if (notificationResult.StatusCode == HttpStatusCode.Accepted)
                {
                    status = Status.Success;
                    isBatchSuccessful = true;
                }

                // Update UnpaidRequest status.
                var updateUnpaidRequestResult = await _unpaidRequestClient.UpdateUnpaidRequestAsync(singleUnpaidRequestToUpdate.UnpaidRequestId, Notification.Push, status, notificationResult.AdditionalErrorMessage, DateTime.UtcNow, correlationId, cancellationToken);
                if (updateUnpaidRequestResult <= 0)
                {
                    _logger.LogWarning((int)LoggingEvents.UpdateItem, "UnpaidEngine.HandleUnpaidRequestAsync - _unpaidRequestClient.UpdateUnpaidRequestAsync returned no results", new { BatchKey = idempotencyKey });
                    isBatchSuccessful = false;
                }

            }

            // Update Batch Status.
            var batchStatus = Status.Failed;
            if (isBatchSuccessful)
            {
                batchStatus = Status.Success;
            }

            var updateUnpaidBatchResult = await _unpaidBatchClient.UpdateUnpaidBatchAsync(idempotencyKey, batchStatus, DateTime.UtcNow, cancellationToken);
            if (updateUnpaidBatchResult <= 0)
            {
                _logger.LogWarning((int)LoggingEvents.UpdateItem, "UnpaidEngine.HandleUnpaidRequestAsync - _unpaidRequestClient.UpdateUnpaidBatchAsync returned no results", new { BatchKey = idempotencyKey });
                isBatchSuccessful = false;
            }

            return isBatchSuccessful;
        }

        public async Task<IEnumerable<UnpaidResponseOutput>> HandleUnpaidResponseAsync(IEnumerable<UnpaidResponseInput> unpaidResponseInputs, CancellationToken cancellationToken)
        {
            if (unpaidResponseInputs == null)
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidEngine.HandleUnpaidResponseAsync - unpaidResponseInputs is null");
                return null;
            }

            var responseInputList = unpaidResponseInputs.ToList();

            if (!responseInputList.Any())
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidEngine.HandleUnpaidResponseAsync - unpaidResponseInputs is empty");
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

                var unpaidRequest = await _unpaidRequestClient.GetUnpaidRequestByIdAsync(unpaidResponseInput, cancellationToken);
                if (unpaidRequest == null)
                {
                    _logger.LogWarning((int)LoggingEvents.GetItem, "UnpaidEngine.HandleUnpaidResponseAsync - _unpaidRequestClient.GetUnpaidRequestByIdAsync returned null", new { CorrelationId = unpaidResponseInput.CorrelationId });
                    unpaidResponseOutputList.Add(unpaidResonseOutput);
                    continue;
                }

                // Check if already exists.
                var unpaidResponse = await _unpaidResponseClient.GetUnpaidResponseAsync(unpaidRequest.UnpaidRequestId, cancellationToken);
                if (unpaidResponse != null && unpaidResponse.Any())
                {
                    _logger.LogWarning((int)LoggingEvents.GetItem, "UnpaidEngine.HandleUnpaidResponseAsync - _unpaidResponseClient.GetUnpaidResponseAsync returned null or empty. Possible cause could be notification response already exists.", new { CorrelationId = unpaidResponseInput.CorrelationId });
                    unpaidResonseOutput.HttpStatusCode = HttpStatusCode.AlreadyReported;
                    unpaidResonseOutput.ErrorMessage = "Notification response already exists.";
                    unpaidResponseOutputList.Add(unpaidResonseOutput);
                    continue;
                }

                var addUnpaidResponseResult = await _unpaidResponseClient.AddPendingUnpaidResponseAsync(unpaidResponseInput, unpaidRequest.UnpaidRequestId, cancellationToken);

                if (addUnpaidResponseResult <= 0)
                {
                    _logger.LogWarning((int)LoggingEvents.InsertItem, "UnpaidEngine.HandleUnpaidResponseAsync - _unpaidResponseClient.AddPendingUnpaidResponseAsync returned no rows", new { CorrelationId = unpaidResponseInput.CorrelationId });
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
