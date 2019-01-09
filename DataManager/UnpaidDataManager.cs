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
        private readonly UnpaidsDBContext _unpaidsDbContext;

        public UnpaidDataManager(UnpaidsDBContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidAsync(IEnumerable<UnpaidDb> unpaids, CancellationToken cancellationToken)
        {
            if (unpaids == null)
            {
                return 0;
            }

            using (_unpaidsDbContext)
            {
                _unpaidsDbContext.Unpaids.AddRange(unpaids);
                return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<UnpaidDb> GetSingleUnpaidAsync(int unpaidId, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.Unpaids.FirstOrDefaultAsync(u => u.UnpaidId == unpaidId, cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidDb>> GetAllUnpaidAsync(string idempotencyKey, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.Unpaids.Where(u => u.IdempotencyKey.Equals(idempotencyKey, StringComparison.InvariantCultureIgnoreCase)).ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidDb>> GetAllUnpaidAsync(CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.Unpaids.ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
