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
        private readonly UnpaidsContext _unpaidsDbContext;

        public UnpaidResponseDataManager(UnpaidsContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidResponseAsync(IEnumerable<TbUnpaidResponse> unpaidResponses, CancellationToken cancellationToken)
        {
            if (unpaidResponses == null)
            {
                return 0;
            }

            _unpaidsDbContext.TbUnpaidResponse.AddRange(unpaidResponses);
            return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TbUnpaidResponse> GetSingleUnpaidResponseAsync(int unpaidResponseId, CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaidResponse.FirstOrDefaultAsync(u => u.UnpaidResponseId == unpaidResponseId, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidResponse>> GetAllUnpaidResponseAsync(int unpaidRequestId, CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaidResponse.Where(u => u.UnpaidRequestId == unpaidRequestId).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidResponse>> GetAllUnpaidResponseAsync(CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaidResponse.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
