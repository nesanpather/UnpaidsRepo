using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DataManager
{
    public class UnpaidBatchDataManager: IUnpaidBatchStorageOperations
    {
        private readonly UnpaidsContext _unpaidsDbContext;

        public UnpaidBatchDataManager(UnpaidsContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidBatchAsync(TbUnpaidBatch unpaidBatch, CancellationToken cancellationToken)
        {
            _unpaidsDbContext.TbUnpaidBatch.Add(unpaidBatch);
            return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidBatch>> GetUnpaidBatchesAsync(int statusId, CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaidBatch.Where(batch => batch.StatusId == statusId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidBatch>> GetUnpaidBatchesAsync(string batchKey, CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaidBatch.Where(batch => batch.BatchKey.Equals(batchKey, StringComparison.InvariantCultureIgnoreCase)).ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateUnpaidBatchAsync(string batchKey, int statusId, DateTime dateModified, CancellationToken cancellationToken)
        {
            var entity = _unpaidsDbContext.TbUnpaidBatch.FirstOrDefault(item => item.BatchKey.Equals(batchKey, StringComparison.InvariantCultureIgnoreCase));

            if (entity == null)
            {
                // Log Warning. No record found for batchKey
                return 0;
            }

            entity.StatusId = statusId;
            entity.DateModified = dateModified;

            _unpaidsDbContext.TbUnpaidBatch.Update(entity);

            return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
