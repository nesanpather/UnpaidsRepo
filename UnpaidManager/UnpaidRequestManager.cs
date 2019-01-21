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
    public class UnpaidRequestManager : IUnpaidRequestClient
    {
        private readonly IUnpaidRequestStorageOperations _unpaidRequestOperations;

        public UnpaidRequestManager(IUnpaidRequestStorageOperations unpaidRequestOperations)
        {
            _unpaidRequestOperations = unpaidRequestOperations;
        }

        public async Task<int> AddUnpaidRequestAsync(IEnumerable<TbUnpaid> unpaids, Notification notification, Status status, CancellationToken cancellationToken)
        {
            if (unpaids == null)
            {
                // Log Error.
                return 0;
            }

            var unpaidDbs = unpaids.ToList();

            if (!unpaidDbs.Any())
            {
                // Log Error.
                return 0;
            }

            var unpaidRequestList = new List<TbUnpaidRequest>();

            foreach (var unpaid in unpaidDbs)
            {
              unpaidRequestList.Add(new TbUnpaidRequest
              {
                  UnpaidId = unpaid.UnpaidId,
                  NotificationId = (int) notification,
                  StatusId = (int) status
              });  
            }

            return await _unpaidRequestOperations.AddUnpaidRequestAsync(unpaidRequestList, cancellationToken);
        }

        public async Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, DateTime dateModified,
            CancellationToken cancellationToken)
        {
            if (unpaidRequestId <= 0)
            {
                // Log Error.
                return 0;
            }

            return await _unpaidRequestOperations.UpdateUnpaidRequestAsync(unpaidRequestId, notification, status, statusAdditionalInfo, dateModified, cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId, CancellationToken cancellationToken)
        {
            if (unpaidId <= 0)
            {
                // Log Error.
                return null;
            }

            return await _unpaidRequestOperations.GetAllUnpaidRequestAsync(unpaidId, cancellationToken);
        }

        public async Task<TbUnpaidRequest> GetLatestSuccessfulUnpaidRequestAsync(UnpaidResponseInput unpaidResponseInput, CancellationToken cancellationToken)
        {
            if (unpaidResponseInput == null)
            {
                // Log Error.
                return null;
            }

            if (string.IsNullOrWhiteSpace(unpaidResponseInput.PolicyNumber))
            {
                // Log Error.
                return null;
            }

            if (string.IsNullOrWhiteSpace(unpaidResponseInput.IdNumber))
            {
                // Log Error.
                return null;
            }

            return await _unpaidRequestOperations.GetSingleUnpaidRequestAsync(unpaidResponseInput.PolicyNumber, unpaidResponseInput.IdNumber, Status.Success, cancellationToken);
        }

        public async Task<IEnumerable<GetAllUnpaidRequestOutput>> GetAllUnpaidRequestAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            var unpaidRequests = await _unpaidRequestOperations.GetUnpaidRequestJoinUnpaidAsync(pageIndex, pageSize, cancellationToken);

            if (unpaidRequests == null)
            {
                return null;
            }

            return GetAllUnpaidRequestOutputs(unpaidRequests);
        }

        public async Task<IEnumerable<GetAllUnpaidRequestOutput>> GetAllUnpaidRequestAsync(CancellationToken cancellationToken)
        {           
            var unpaidRequests = await _unpaidRequestOperations.GetUnpaidRequestJoinUnpaidAsync(cancellationToken);

            if (unpaidRequests == null)
            {
                return null;
            }

            return GetAllUnpaidRequestOutputs(unpaidRequests);
        }

        public async Task<IEnumerable<GetAllUnpaidRequestOutput>> GetAllUnpaidRequestAsync(string policyNumber, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                // Log Error.
                return null;
            }

            var unpaidRequests = await _unpaidRequestOperations.GetUnpaidRequestJoinUnpaidAsync(cancellationToken);

            if (unpaidRequests == null)
            {
                return null;
            }

            return GetAllUnpaidRequestOutputs(unpaidRequests.Where(u => string.Equals(u.Unpaid.PolicyNumber, policyNumber, StringComparison.InvariantCultureIgnoreCase)));
        }

        public async Task<IEnumerable<GetAllUnpaidRequestOutput>> GetAllUnpaidRequestAsync(DateTime dateFrom, DateTime dateTo, DateType dateType, CancellationToken cancellationToken)
        {
            if (dateFrom == DateTime.MinValue || dateFrom == DateTime.MaxValue)
            {
                // Log Error.

                return await GetAllUnpaidRequestAsync(cancellationToken);
            }

            if (dateTo == DateTime.MinValue || dateTo == DateTime.MaxValue)
            {
                // Log Error.

                return await GetAllUnpaidRequestAsync(cancellationToken);
            }

            if (dateType <= 0)
            {
                dateType = DateType.DateAdded;
            }

            var unpaidRequests = await _unpaidRequestOperations.GetUnpaidRequestJoinUnpaidAsync(cancellationToken);

            if (unpaidRequests == null)
            {
                return null;
            }

            dateFrom = dateFrom.StartOfDay();
            dateTo = dateTo.EndOfDay();

            switch (dateType)
            {
                case DateType.DateAdded:
                {
                    return GetAllUnpaidRequestOutputs(unpaidRequests.Where(u => u.Unpaid.DateCreated >= dateFrom && u.Unpaid.DateCreated <= dateTo));
                }
                case DateType.DateNotificationSent:
                {
                    // TODO: Change to DateModified once api and integration tests are all working.
                    return GetAllUnpaidRequestOutputs(unpaidRequests.Where(u => u.DateCreated >= dateFrom && u.DateCreated <= dateTo));
                }
            }

            // Log Warning. Invalid DateType. 
            return await GetAllUnpaidRequestAsync(cancellationToken);
        }

        private static IEnumerable<GetAllUnpaidRequestOutput> GetAllUnpaidRequestOutputs(IEnumerable<TbUnpaidRequest> unpaidRequests)
        {
            var tbUnpaidRequests = unpaidRequests.ToList();

            if (!tbUnpaidRequests.Any())
            {
                return null;
            }

            var unpaidRequestOutputList = new List<GetAllUnpaidRequestOutput>();

            foreach (var unpaidRequest in tbUnpaidRequests)
            {
                unpaidRequestOutputList.Add(new GetAllUnpaidRequestOutput
                {
                    UnpaidId = unpaidRequest.UnpaidId,
                    PolicyNumber = unpaidRequest.Unpaid.PolicyNumber,
                    IdNumber = unpaidRequest.Unpaid.IdNumber,
                    Name = unpaidRequest.Unpaid.Name,
                    Message = unpaidRequest.Unpaid.Message,
                    DateAdded = unpaidRequest.Unpaid.DateCreated,
                    NotificationRequestId = unpaidRequest.UnpaidRequestId,
                    NotificationType = unpaidRequest.Notification.Notification,
                    DateNotificationSent = unpaidRequest.DateCreated,
                    NotificationSentStatus = unpaidRequest.Status.Status,
                    NotificationErrorMessage = unpaidRequest.StatusAdditionalInfo
                });
            }

            return unpaidRequestOutputList;
        }
    }
}
