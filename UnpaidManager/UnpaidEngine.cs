using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager
{
    public class UnpaidEngine: IUnpaidEngineHandler
    {
        private readonly IUnpaidClient _unpaidClient;
        private readonly IUnpaidRequestClient _unpaidRequestClient;
        private readonly INotification _notification;

        public UnpaidEngine(IUnpaidClient unpaidClient, IUnpaidRequestClient unpaidRequestClient, INotification notification)
        {
            _unpaidClient = unpaidClient;
            _unpaidRequestClient = unpaidRequestClient;
            _notification = notification;
        }

        public async Task<IEnumerable<UnpaidOutput>> HandleUnpaidAsync(IEnumerable<Unpaid> unpaids, string idempotencyKey, CancellationToken cancellationToken)
        {
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

            // Add UnpaidRequest to storage and default status as pending.
            var unpaidRequestResult = await _unpaidRequestClient.AddUnpaidRequestAsync(byIdempotencyResultList, Notification.Push, Status.Pending, cancellationToken);

            if (unpaidRequestResult <= 0)
            {
                // Log Error.
                return null;
            }

            return await HandleUnpaidRequestAsync(byIdempotencyResultList, cancellationToken);
        }

        private async Task<IEnumerable<UnpaidOutput>> HandleUnpaidRequestAsync(IEnumerable<UnpaidDb> unpaids, CancellationToken cancellationToken)
        {
            var unpaidOutputList = new List<UnpaidOutput>();

            foreach (var unpaid in unpaids)
            {
                // create notification task.
                var notificationTask = _notification.SendAsync($"Dear {unpaid.Name}", unpaid.Message, unpaid.IdNumber, cancellationToken);

                // Get all UnpaidRequests by idempotencyKey
                var unpaidRequestsToUpdate = await _unpaidRequestClient.GetAllUnpaidRequestAsync(unpaid.UnpaidId, cancellationToken);

                if (unpaidRequestsToUpdate == null)
                {
                    // Log Warning. No UnpaidRequests to update.
                    return null;
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

                    unpaidOutputList.Add(new UnpaidOutput
                    {
                        PolicyNumber = unpaid.PolicyNumber,
                        IdNumber = unpaid.IdNumber,
                        Name = unpaid.Name,
                        Message = unpaid.Message,
                        Status = status.ToString(),
                        ErrorMessage = notificationResult.AdditionalErrorMessage
                    });
                }
            }

            return unpaidOutputList;
        }
    }
}
