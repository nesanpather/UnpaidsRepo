using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidRequestClient
    {
        Task<int> AddUnpaidRequestAsync(IEnumerable<TbUnpaid> unpaids, Notification notification, Status status, CancellationToken cancellationToken);

        Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, DateTime dateModified, string correlationId, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId, Status status, CancellationToken cancellationToken);
        Task<TbUnpaidRequest> GetUnpaidRequestByIdAsync(UnpaidResponseInput unpaidResponseInput, CancellationToken cancellationToken);
        Task<IEnumerable<GetAllUnpaidRequestOutput>> GetAllUnpaidRequestAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<IEnumerable<GetAllUnpaidRequestOutput>> GetAllUnpaidRequestAsync(CancellationToken cancellationToken);
        Task<IEnumerable<GetAllUnpaidRequestOutput>> GetAllUnpaidRequestAsync(string policyNumber, CancellationToken cancellationToken);
        Task<IEnumerable<GetAllUnpaidRequestOutput>> GetAllUnpaidRequestAsync(DateTime dateFrom, DateTime dateTo, DateType dateType, CancellationToken cancellationToken);
    }
}
