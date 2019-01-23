using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidRequestStorageOperations
    {
        Task<int> AddUnpaidRequestAsync(IEnumerable<TbUnpaidRequest> unpaidRequests, CancellationToken cancellationToken);
        Task<TbUnpaidRequest> GetSingleUnpaidRequestAsync(int unpaidRequestId, CancellationToken cancellationToken);
        Task<TbUnpaidRequest> GetSingleUnpaidRequestAsync(string idNumber, int unpaidRequestId, Status status, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidRequest>> GetAllUnpaidRequestAsync(CancellationToken cancellationToken);
        Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, DateTime dateModified, string correlationId,
            CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidRequest>> GetUnpaidRequestJoinUnpaidAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidRequest>> GetUnpaidRequestJoinUnpaidAsync(CancellationToken cancellationToken);        
    }
}