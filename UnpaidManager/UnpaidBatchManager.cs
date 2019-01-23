using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager
{
    public class UnpaidBatchManager: IUnpaidBatchClient
    {
        private readonly IUnpaidBatchStorageOperations _unpaidBatchStorageOperations;

        public UnpaidBatchManager(IUnpaidBatchStorageOperations unpaidBatchStorageOperations)
        {
            _unpaidBatchStorageOperations = unpaidBatchStorageOperations;
        }

        public async Task<int> AddUnpaidBatchAsync(string batchKey, Status status, string userName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(batchKey))
            {
                // Log Error.
                return 0;
            }

            var newBatch = new TbUnpaidBatch
            {
                BatchKey = batchKey,
                StatusId = (int) status,
                UserName = userName
            };

            return await _unpaidBatchStorageOperations.AddUnpaidBatchAsync(newBatch, cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidBatch>> GetUnpaidBatchByBatchKeyAsync(string batchKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(batchKey))
            {
                // Log Error.
                return null;
            }

            return await _unpaidBatchStorageOperations.GetUnpaidBatchesAsync(batchKey, cancellationToken);
        }

        public async Task<int> UpdateUnpaidBatchAsync(string batchKey, Status status, DateTime dateModified, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(batchKey))
            {
                // Log Error.
                return 0;
            }

            return await _unpaidBatchStorageOperations.UpdateUnpaidBatchAsync(batchKey, (int) status, dateModified, cancellationToken);
        }
    }
}
