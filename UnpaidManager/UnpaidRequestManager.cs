using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using Microsoft.Extensions.Logging;
using UnpaidManager.Interfaces;
using UnpaidModels;
using Utilities;

namespace UnpaidManager
{
    public class UnpaidRequestManager : IUnpaidRequestClient
    {
        private readonly IUnpaidRequestStorageOperations _unpaidRequestOperations;
        private readonly ILogger<UnpaidRequestManager> _logger;

        public UnpaidRequestManager(IUnpaidRequestStorageOperations unpaidRequestOperations, ILogger<UnpaidRequestManager> logger)
        {
            _unpaidRequestOperations = unpaidRequestOperations;
            _logger = logger;
        }

        public async Task<int> AddUnpaidRequestAsync(IEnumerable<TbUnpaid> unpaids, Notification notification, Status status, CancellationToken cancellationToken)
        {
            if (unpaids == null)
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.AddUnpaidRequestAsync - unpaids is null");
                return 0;
            }

            var unpaidDbs = unpaids.ToList();

            if (!unpaidDbs.Any())
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.AddUnpaidRequestAsync - unpaids is empty");
                return 0;
            }

            var unpaidRequestList = new List<TbUnpaidRequest>();

            foreach (var unpaid in unpaidDbs)
            {
              unpaidRequestList.Add(new TbUnpaidRequest
              {
                  UnpaidId = unpaid.UnpaidId,
                  NotificationId = (int) notification,
                  StatusId = (int) status,
                  CorrelationId = string.Empty
              });  
            }

            return await _unpaidRequestOperations.AddUnpaidRequestAsync(unpaidRequestList, cancellationToken);
        }

        public async Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, DateTime dateModified, string correlationId, CancellationToken cancellationToken)
        {
            if (unpaidRequestId <= 0)
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.UpdateUnpaidRequestAsync - unpaidRequestId is less than or equal to zero");
                return 0;
            }

            return await _unpaidRequestOperations.UpdateUnpaidRequestAsync(unpaidRequestId, notification, status, statusAdditionalInfo, dateModified, correlationId, cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId, Status status, CancellationToken cancellationToken)
        {
            if (unpaidId <= 0)
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetAllUnpaidRequestAsync - unpaidId is less than or equal to zero");
                return null;
            }

            var unpaidRequests = await _unpaidRequestOperations.GetAllUnpaidRequestAsync(unpaidId, cancellationToken);

            return unpaidRequests.Where(request => request.StatusId == (int) status);
        }

        public async Task<TbUnpaidRequest> GetUnpaidRequestByIdAsync(UnpaidResponseInput unpaidResponseInput, CancellationToken cancellationToken)
        {
            if (unpaidResponseInput == null)
            {
                _logger.LogWarning((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetUnpaidRequestByIdAsync - unpaidResponseInput is null");
                return null;
            }

            if (string.IsNullOrWhiteSpace(unpaidResponseInput.PolicyNumber))
            {
                _logger.LogWarning((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetUnpaidRequestByIdAsync - PolicyNumber is null or empty");
                return null;
            }

            if (string.IsNullOrWhiteSpace(unpaidResponseInput.IdNumber))
            {
                _logger.LogWarning((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetUnpaidRequestByIdAsync - IdNumber is null or empty");
                return null;
            }

            if (string.IsNullOrWhiteSpace(unpaidResponseInput.CorrelationId))
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetUnpaidRequestByIdAsync - CorrelationId is null or empty");
                return null;
            }

            var correlationIdSplit = unpaidResponseInput.CorrelationId.Split("_");
            if (correlationIdSplit.Length < 2)
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetUnpaidRequestByIdAsync - CorrelationId is not in the correct format");
                return null;
            }

            return await _unpaidRequestOperations.GetSingleUnpaidRequestAsync(unpaidResponseInput.IdNumber, Convert.ToInt32(correlationIdSplit[1]), Status.Success, cancellationToken);
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
                _logger.LogWarning((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetAllUnpaidRequestAsync - policyNumber is null or empty");
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
                _logger.LogWarning((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetAllUnpaidRequestAsync - dateFrom is not set");

                return await GetAllUnpaidRequestAsync(cancellationToken);
            }

            if (dateTo == DateTime.MinValue || dateTo == DateTime.MaxValue)
            {
                _logger.LogWarning((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetAllUnpaidRequestAsync - dateTo is not set");

                return await GetAllUnpaidRequestAsync(cancellationToken);
            }

            if (dateType <= 0)
            {
                dateType = DateType.DateAdded;
            }

            var unpaidRequests = await _unpaidRequestOperations.GetUnpaidRequestJoinUnpaidAsync(cancellationToken);

            if (unpaidRequests == null)
            {
                _logger.LogWarning((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetAllUnpaidRequestAsync - _unpaidRequestOperations.GetUnpaidRequestJoinUnpaidAsync returned null");
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

            _logger.LogWarning((int)LoggingEvents.ValidationFailed, "UnpaidRequestManager.GetAllUnpaidRequestAsync - invalid dateType");
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
                    NotificationErrorMessage = unpaidRequest.StatusAdditionalInfo,
                    CorrelationId = unpaidRequest.CorrelationId,
                });
            }

            return unpaidRequestOutputList;
        }
    }
}
