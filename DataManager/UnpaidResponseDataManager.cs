using System.Collections.Generic;
using System.Linq;
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

        public async Task<int> AddUnpaidResponseAsync(IEnumerable<UnpaidResponse> unpaidResponses)
        {
            if (unpaidResponses == null)
            {
                return 0;
            }

            using (_unpaidsDbContext)
            {
                _unpaidsDbContext.UnpaidResponses.AddRange(unpaidResponses);
                return await _unpaidsDbContext.SaveChangesAsync();
            }
        }

        public async Task<UnpaidResponse> GetSingleUnpaidResponseAsync(int unpaidResponseId)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidResponses.FirstOrDefaultAsync(u => u.UnpaidResponseId == unpaidResponseId);
            }
        }

        public async Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync(int unpaidRequestId)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidResponses.Where(u => u.UnpaidRequestId == unpaidRequestId).ToListAsync();
            }
        }

        public async Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync()
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidResponses.ToListAsync();
            }
        }
    }
}
