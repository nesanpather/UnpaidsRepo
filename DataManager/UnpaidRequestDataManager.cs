using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using Microsoft.EntityFrameworkCore;
using UnpaidModels;

namespace DataManager
{
    public class UnpaidRequestDataManager: IUnpaidRequestOperations
    {
        private readonly UnpaidsDBContext _unpaidsDbContext;

        public UnpaidRequestDataManager(UnpaidsDBContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidRequestAsync(IEnumerable<UnpaidRequest> unpaidRequests, CancellationToken cancellationToken)
        {
            if (unpaidRequests == null)
            {
                return 0;
            }

            using (_unpaidsDbContext)
            {
                _unpaidsDbContext.UnpaidRequests.AddRange(unpaidRequests);
                return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<UnpaidRequest> GetSingleUnpaidRequestAsync(int unpaidRequestId, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidRequests.FirstOrDefaultAsync(u => u.UnpaidRequestId == unpaidRequestId, cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidRequests.Where(u => u.UnpaidId == unpaidId).ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync(CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidRequests.ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
