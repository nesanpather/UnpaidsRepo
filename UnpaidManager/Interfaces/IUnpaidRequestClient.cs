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

        Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId, CancellationToken cancellationToken);
    }
}
