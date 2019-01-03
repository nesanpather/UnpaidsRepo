using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<int> AddUnpaidRequestAsync(IEnumerable<UnpaidRequest> unpaidRequests)
        {
            if (unpaidRequests == null)
            {
                return 0;
            }

            using (_unpaidsDbContext)
            {
                _unpaidsDbContext.UnpaidRequests.AddRange(unpaidRequests);
                return await _unpaidsDbContext.SaveChangesAsync();
            }
        }

        public async Task<UnpaidRequest> GetSingleUnpaidRequestAsync(int unpaidRequestId)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidRequests.FirstOrDefaultAsync(u => u.UnpaidRequestId == unpaidRequestId);
            }
        }

        public async Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidRequests.Where(u => u.UnpaidId == unpaidId).ToListAsync();
            }
        }

        public async Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync()
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidRequests.ToListAsync();
            }
        }
    }
}
