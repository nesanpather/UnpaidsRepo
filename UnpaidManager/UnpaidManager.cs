using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using Microsoft.Extensions.Logging;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager
{
    public class UnpaidManager: IUnpaidClient
    {
        private readonly IUnpaidStorageOperations _unpaidOperations;
        private readonly ILogger<UnpaidManager> _logger;

        public UnpaidManager(IUnpaidStorageOperations unpaidOperations, ILogger<UnpaidManager> logger)
        {
            _unpaidOperations = unpaidOperations;
            _logger = logger;
        }

        public async Task<int> AddUnpaidAsync(IEnumerable<UnpaidInput> unpaids, int unpaidBatchId, CancellationToken cancellationToken)
        {
            if (unpaids == null)
            {
                _logger.LogError((int) LoggingEvents.ValidationFailed, "UnpaidManager.AddUnpaidAsync - unpaids is null");
                return 0;
            }

            if (unpaidBatchId <= 0)
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidManager.AddUnpaidAsync - unpaidBatchId is less than or equal to zero");
                return 0;
            }

            var enumerable = unpaids.ToList();

            if (!enumerable.Any())
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidManager.AddUnpaidAsync - unpaids is empty");
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
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidManager.GetUnpaidsByIdempotencyKeyAsync - idempotencyKey is null or empty");
                return null;
            }

            return await _unpaidOperations.GetAllUnpaidAsync(idempotencyKey, cancellationToken);
        }
    }
}
