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
    }
}
