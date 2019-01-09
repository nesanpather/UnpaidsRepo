using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidRequestClient
    {
        Task<int> AddUnpaidRequestAsync(IEnumerable<UnpaidDb> unpaids, Notification notification, Status status, CancellationToken cancellationToken);

        Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidRequestDb>> GetAllUnpaidRequestAsync(int unpaidId, CancellationToken cancellationToken);
    }
}
