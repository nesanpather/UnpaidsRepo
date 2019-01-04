using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using Microsoft.EntityFrameworkCore;
using UnpaidModels;

namespace DataManager
{
    public class UnpaidResponseDataManager: IUnpaidResponseOperations
    {
        private readonly UnpaidsDBContext _unpaidsDbContext;

        public UnpaidResponseDataManager(UnpaidsDBContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidResponseAsync(IEnumerable<UnpaidResponse> unpaidResponses, CancellationToken cancellationToken)
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

        public async Task<UnpaidResponse> GetSingleUnpaidResponseAsync(int unpaidResponseId, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidResponses.FirstOrDefaultAsync(u => u.UnpaidResponseId == unpaidResponseId, cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync(int unpaidRequestId, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidResponses.Where(u => u.UnpaidRequestId == unpaidRequestId).ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync(CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidResponses.ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
