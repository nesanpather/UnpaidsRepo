using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataManager.Interfaces;
using Microsoft.EntityFrameworkCore;
using UnpaidModels;

namespace DataManager
{
    public class UnpaidDataManager: IUnpaidOperations
    {
        private readonly UnpaidsDBContext _unpaidsDbContext;

        public UnpaidDataManager(UnpaidsDBContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidAsync(IEnumerable<Unpaid> unpaids)
        {
            if (unpaids == null)
            {
                return 0;
            }

            using (_unpaidsDbContext)
            {
                _unpaidsDbContext.Unpaids.AddRange(unpaids);
                return await _unpaidsDbContext.SaveChangesAsync();
            }
        }

        public async Task<Unpaid> GetSingleUnpaidAsync(int unpaidId)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.Unpaids.FirstOrDefaultAsync(u => u.UnpaidId == unpaidId);
            }
        }

        public async Task<IEnumerable<Unpaid>> GetAllUnpaidAsync(string policyNumber)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.Unpaids.Where(u => u.PolicyNumber.Equals(policyNumber, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
            }
        }

        public async Task<IEnumerable<Unpaid>> GetAllUnpaidAsync()
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.Unpaids.ToListAsync();
            }
        }
    }
}
