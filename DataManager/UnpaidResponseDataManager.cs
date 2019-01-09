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
    public class UnpaidResponseDataManager: IUnpaidResponseStorageOperations
    {
        private readonly UnpaidsDBContext _unpaidsDbContext;

        public UnpaidResponseDataManager(UnpaidsDBContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidResponseAsync(IEnumerable<UnpaidResponseDb> unpaidResponses, CancellationToken cancellationToken)
        {
            if (unpaidResponses == null)
            {
                return 0;
            }

            using (_unpaidsDbContext)
            {
                _unpaidsDbContext.UnpaidResponses.AddRange(unpaidResponses);
                return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<UnpaidResponseDb> GetSingleUnpaidResponseAsync(int unpaidResponseId, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidResponses.FirstOrDefaultAsync(u => u.UnpaidResponseId == unpaidResponseId, cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidResponseDb>> GetAllUnpaidResponseAsync(int unpaidRequestId, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidResponses.Where(u => u.UnpaidRequestId == unpaidRequestId).ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidResponseDb>> GetAllUnpaidResponseAsync(CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidResponses.ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
