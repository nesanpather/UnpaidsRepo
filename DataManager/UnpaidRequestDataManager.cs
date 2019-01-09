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
    public class UnpaidRequestDataManager: IUnpaidRequestStorageOperations
    {
        private readonly UnpaidsDBContext _unpaidsDbContext;

        public UnpaidRequestDataManager(UnpaidsDBContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidRequestAsync(IEnumerable<UnpaidRequestDb> unpaidRequests, CancellationToken cancellationToken)
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

        public async Task<UnpaidRequestDb> GetSingleUnpaidRequestAsync(int unpaidRequestId, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidRequests.FirstOrDefaultAsync(u => u.UnpaidRequestId == unpaidRequestId, cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidRequestDb>> GetAllUnpaidRequestAsync(int unpaidId, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidRequests.Where(u => u.UnpaidId == unpaidId).ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<UnpaidRequestDb>> GetAllUnpaidRequestAsync(CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                return await _unpaidsDbContext.UnpaidRequests.ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, CancellationToken cancellationToken)
        {
            using (_unpaidsDbContext)
            {
                var entity = _unpaidsDbContext.UnpaidRequests.FirstOrDefault(item => item.UnpaidRequestId == unpaidRequestId);

                if (entity == null)
                {
                    // Log Warning. No record found for unpaidRequestId
                    return 0;
                }

                entity.NotificationId = (int) notification;
                entity.StatusId = (int) status;
                entity.StatusAdditionalInfo = statusAdditionalInfo;

                _unpaidsDbContext.UnpaidRequests.Update(entity);
                return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
