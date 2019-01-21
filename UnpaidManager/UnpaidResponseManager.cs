using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using UnpaidManager.Interfaces;
using UnpaidModels;
using Utilities;

namespace UnpaidManager
{
    public class UnpaidResponseManager: IUnpaidResponseClient
    {
        private readonly IUnpaidResponseStorageOperations _unpaidResponseStorageOperations;
        private readonly IUnpaidRequestStorageOperations _unpaidRequestStorageOperations;

        public UnpaidResponseManager(IUnpaidResponseStorageOperations unpaidResponseStorageOperations, IUnpaidRequestStorageOperations unpaidRequestStorageOperations)
        {
            _unpaidResponseStorageOperations = unpaidResponseStorageOperations;
            _unpaidRequestStorageOperations = unpaidRequestStorageOperations;
        }

        public async Task<int> AddPendingUnpaidResponseAsync(UnpaidResponseInput unpaidResponseInput,int unpaidRequestId, CancellationToken cancellationToken)
        {
            if (unpaidResponseInput == null)
            {
                // Log Error.
                return 0;
            }

            var unpaidResponse = new List<TbUnpaidResponse>
            {
                new TbUnpaidResponse
                {
                    UnpaidRequestId = unpaidRequestId,
                    ResponseId = (int)unpaidResponseInput.ContactOption,
                    Accepted = unpaidResponseInput.Accepted,
                    StatusId = (int)Status.Pending
                }
            };

            return await _unpaidResponseStorageOperations.AddUnpaidResponseAsync(unpaidResponse, cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidResponse>> GetUnpaidResponseAsync(int unpaidRequestId, CancellationToken cancellationToken)
        {
            if (unpaidRequestId <= 0)
            {
                // Log Error.
                return null;
            }

            return await _unpaidResponseStorageOperations.GetAllUnpaidResponseAsync(unpaidRequestId, cancellationToken);
        }

        public async Task<IEnumerable<GetAllUnpaidResponseOutput>> GetAllUnpaidResponseAsync(CancellationToken cancellationToken)
        {
            var unpaidResponsesTask = _unpaidResponseStorageOperations.GetAllUnpaidResponseJoinUnpaidRequest(cancellationToken);
            var unpaidRequestsJoinUnpaidTask = _unpaidRequestStorageOperations.GetUnpaidRequestJoinUnpaidAsync(cancellationToken);

            var unpaidResponseTaskResult = await unpaidResponsesTask;

            if (unpaidResponseTaskResult == null)
            {
                unpaidRequestsJoinUnpaidTask.Dispose();
                return null;
            }

            var responseTaskResult = unpaidResponseTaskResult.ToList();
            if (!responseTaskResult.Any())
            {
                unpaidRequestsJoinUnpaidTask.Dispose();
                return null;
            }

            var unpaidRequestsJoinUnpaidTaskResult = await unpaidRequestsJoinUnpaidTask;

            if (unpaidRequestsJoinUnpaidTaskResult == null)
            {
                return null;
            }

            var requestsJoinUnpaidTaskResult = unpaidRequestsJoinUnpaidTaskResult.ToList();
            if (!requestsJoinUnpaidTaskResult.Any())
            {
                return null;
            }

            var query = from a in responseTaskResult
                join tbUnpaidRequest in requestsJoinUnpaidTaskResult on a.UnpaidRequestId equals tbUnpaidRequest.UnpaidRequestId
                select new GetAllUnpaidResponseOutput
                {
                    UnpaidId = tbUnpaidRequest.UnpaidId, PolicyNumber = tbUnpaidRequest.Unpaid.PolicyNumber, IdNumber = tbUnpaidRequest.Unpaid.IdNumber,
                    DateAdded = tbUnpaidRequest.Unpaid.DateCreated, NotificationRequestId = tbUnpaidRequest.UnpaidRequestId,
                    NotificationType = tbUnpaidRequest.Notification.Notification,
                    DateNotificationSent = tbUnpaidRequest.DateCreated, ContactOptionType = a.Response.Response, ContactOptionStatus = a.Status.Status, Accepted = a.Accepted,
                    DateNotificationResponseAdded = a.DateCreated
                };

            return query;
        }

        public async Task<IEnumerable<GetAllUnpaidResponseOutput>> GetAllUnpaidResponseAsync(string policyNumber, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                // Log Error.
                return null;
            }

            var getAllUnpaidResponseResult = await GetAllUnpaidResponseAsync(cancellationToken);

            // TODO: There is a more efficient way of doing this.
            return getAllUnpaidResponseResult?.Where(u => string.Equals(u.PolicyNumber, policyNumber));
        }

        public async Task<IEnumerable<GetAllUnpaidResponseOutput>> GetAllUnpaidResponseAsync(DateTime dateFrom, DateTime dateTo, DateType dateType, CancellationToken cancellationToken)
        {
            if (dateFrom == DateTime.MinValue || dateFrom == DateTime.MaxValue)
            {
                // Log Error.

                return await GetAllUnpaidResponseAsync(cancellationToken);
            }

            if (dateTo == DateTime.MinValue || dateTo == DateTime.MaxValue)
            {
                // Log Error.

                return await GetAllUnpaidResponseAsync(cancellationToken);
            }

            if (dateType <= 0)
            {
                dateType = DateType.DateNotificationResponseAdded;
            }

            var getAllUnpaidResponseResult = await GetAllUnpaidResponseAsync(cancellationToken);
            if (getAllUnpaidResponseResult == null)
            {
                // Log Error.
                return null;
            }

            dateFrom = dateFrom.StartOfDay();
            dateTo = dateTo.EndOfDay();

            switch (dateType)
            {
                case DateType.DateAdded:
                {
                    return getAllUnpaidResponseResult.Where(ur => ur.DateAdded >= dateFrom && ur.DateAdded <= dateTo);
                }
                case DateType.DateNotificationSent:
                {
                    return getAllUnpaidResponseResult.Where(ur => ur.DateNotificationSent >= dateFrom && ur.DateNotificationSent <= dateTo);
                }
                case DateType.DateNotificationResponseAdded:
                {
                    return getAllUnpaidResponseResult.Where(ur => ur.DateNotificationResponseAdded >= dateFrom && ur.DateNotificationResponseAdded <= dateTo);
                }
            }

            // Log Warning. Invalid DateType.
            return getAllUnpaidResponseResult;
        }
    }
}
