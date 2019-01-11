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
    public class UnpaidResponseManager: IUnpaidResponseClient
    {
        private readonly IUnpaidResponseStorageOperations _unpaidResponseStorageOperations;

        public UnpaidResponseManager(IUnpaidResponseStorageOperations unpaidResponseStorageOperations)
        {
            _unpaidResponseStorageOperations = unpaidResponseStorageOperations;
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
    }
}
