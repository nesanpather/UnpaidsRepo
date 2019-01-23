using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager
{
    public class UnpaidManager: IUnpaidClient
    {
        private readonly IUnpaidStorageOperations _unpaidOperations;

        public UnpaidManager(IUnpaidStorageOperations unpaidOperations)
        {
            _unpaidOperations = unpaidOperations;
        }

        public async Task<int> AddUnpaidAsync(IEnumerable<UnpaidInput> unpaids, int unpaidBatchId, CancellationToken cancellationToken)
        {
            if (unpaids == null)
            {
                // Log Error.
                return 0;
            }

            if (unpaidBatchId <= 0)
            {
                // Log Error.
                return 0;
            }

            var enumerable = unpaids.ToList();

            if (!enumerable.Any())
            {
                // Log Error.
                return 0;
            }

            var unpaidDbList = new List<TbUnpaid>();

            foreach (var unpaid in enumerable)
            {
                unpaidDbList.Add(new TbUnpaid
                {
                    PolicyNumber = unpaid.PolicyNumber,
                    IdNumber = unpaid.IdNumber,
                    Name = unpaid.Name,
                    Message = unpaid.Message,
                    Title = unpaid.MessageTitle,
                    UnpaidBatchId = unpaidBatchId
                });
            }

            var addUnpaidResult = await _unpaidOperations.AddUnpaidAsync(unpaidDbList, cancellationToken);
            return addUnpaidResult;
        }

        public async Task<IEnumerable<TbUnpaid>> GetUnpaidsByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                // Log Error.
                return null;
            }

            return await _unpaidOperations.GetAllUnpaidAsync(idempotencyKey, cancellationToken);
        }
    }
}
