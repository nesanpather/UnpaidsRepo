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

        public async Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, CancellationToken cancellationToken)
        {
            if (unpaidRequestId <= 0)
            {
                // Log Error.
                return 0;
            }

            return await _unpaidRequestOperations.UpdateUnpaidRequestAsync(unpaidRequestId, notification, status, statusAdditionalInfo, cancellationToken);
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
    }
}
