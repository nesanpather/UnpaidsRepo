using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Remotion.Linq.Clauses;
using UnpaidModels;

namespace DataManager
{
    public class UnpaidRequestDataManager: IUnpaidRequestStorageOperations
    {
        private readonly UnpaidsContext _unpaidsDbContext;

        public UnpaidRequestDataManager(UnpaidsContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidRequestAsync(IEnumerable<TbUnpaidRequest> unpaidRequests, CancellationToken cancellationToken)
        {
            if (unpaidRequests == null)
            {
                return 0;
            }

            _unpaidsDbContext.TbUnpaidRequest.AddRange(unpaidRequests);
            return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TbUnpaidRequest> GetSingleUnpaidRequestAsync(int unpaidRequestId, CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaidRequest.FirstOrDefaultAsync(u => u.UnpaidRequestId == unpaidRequestId, cancellationToken: cancellationToken);
        }

        public async Task<TbUnpaidRequest> GetSingleUnpaidRequestAsync(string idNumber, int unpaidRequestId, Status status, CancellationToken cancellationToken)
        {
            // TODO: Validate against BatchKey.
            var query = from ur in _unpaidsDbContext.TbUnpaidRequest
                join u in _unpaidsDbContext.TbUnpaid on ur.UnpaidId equals u.UnpaidId
                where ur.StatusId == (int)status
                      && ur.UnpaidRequestId == unpaidRequestId
                      && string.Equals(u.IdNumber, idNumber, StringComparison.InvariantCultureIgnoreCase) 
                select ur;

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId, CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaidRequest.Where(u => u.UnpaidId == unpaidId).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidRequest>> GetAllUnpaidRequestAsync(CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbUnpaidRequest.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidRequest>> GetUnpaidRequestJoinUnpaidAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var query = from ur in _unpaidsDbContext.TbUnpaidRequest
                join u in _unpaidsDbContext.TbUnpaid on ur.UnpaidId equals u.UnpaidId
                join s in _unpaidsDbContext.TbStatus on ur.StatusId equals s.StatusId
                join n in _unpaidsDbContext.TbNotification on ur.NotificationId equals n.NotificationId
                select new TbUnpaidRequest{Unpaid = u, UnpaidRequestId = ur.UnpaidRequestId, UnpaidId = ur.UnpaidId, Notification = n, Status = s, DateCreated = ur.DateCreated, StatusAdditionalInfo = ur.StatusAdditionalInfo, CorrelationId = ur.CorrelationId };

            return await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TbUnpaidRequest>> GetUnpaidRequestJoinUnpaidAsync(CancellationToken cancellationToken)
        {
            var query = from ur in _unpaidsDbContext.TbUnpaidRequest
                join u in _unpaidsDbContext.TbUnpaid on ur.UnpaidId equals u.UnpaidId
                join s in _unpaidsDbContext.TbStatus on ur.StatusId equals s.StatusId
                join n in _unpaidsDbContext.TbNotification on ur.NotificationId equals n.NotificationId
                select new TbUnpaidRequest { Unpaid = u, UnpaidRequestId = ur.UnpaidRequestId, UnpaidId = ur.UnpaidId, Notification = n, Status = s, DateCreated = ur.DateCreated, StatusAdditionalInfo = ur.StatusAdditionalInfo, CorrelationId = ur.CorrelationId};

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, DateTime dateModified, string correlationId, CancellationToken cancellationToken)
        {
            var entity = _unpaidsDbContext.TbUnpaidRequest.FirstOrDefault(item => item.UnpaidRequestId == unpaidRequestId);

            if (entity == null)
            {
                // Log Warning. No record found for unpaidRequestId
                return 0;
            }

            entity.NotificationId = (int) notification;
            entity.StatusId = (int) status;
            entity.StatusAdditionalInfo = statusAdditionalInfo;
            entity.DateModified = dateModified;
            entity.CorrelationId = correlationId;

            _unpaidsDbContext.TbUnpaidRequest.Update(entity);
            return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
