using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using Microsoft.EntityFrameworkCore;
using UnpaidModels;

namespace DataManager
{
    public class UnpaidDataManager: IUnpaidStorageOperations
    {
        private readonly UnpaidsContext _unpaidsDbContext;

        public UnpaidDataManager(UnpaidsContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidAsync(IEnumerable<TbUnpaid> unpaids, CancellationToken cancellationToken)
        {
            if (unpaids == null)
            {
                return 0;
            }

            _unpaidsDbContext.TbUnpaid.AddRange(unpaids);
            return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TbUnpaid> GetSingleUnpaidAsync(int unpaidId, CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaid.FirstOrDefaultAsync(u => u.UnpaidId == unpaidId, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaid>> GetAllUnpaidAsync(string batchKey, CancellationToken cancellationToken)
        {
            var query = from u in _unpaidsDbContext.TbUnpaid
                        join ub in _unpaidsDbContext.TbUnpaidBatch on u.UnpaidBatchId equals ub.UnpaidBatchId
                        where ub.BatchKey.Equals(batchKey, StringComparison.InvariantCultureIgnoreCase) && ub.StatusId == (int) Status.Pending
                        select u;
                
            return await query.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaid>> GetAllUnpaidAsync(CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaid.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
