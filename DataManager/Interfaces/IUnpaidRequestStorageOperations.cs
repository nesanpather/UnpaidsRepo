using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidRequestStorageOperations
    {
        Task<int> AddUnpaidRequestAsync(IEnumerable<UnpaidRequestDb> unpaidRequests, CancellationToken cancellationToken);
        Task<UnpaidRequestDb> GetSingleUnpaidRequestAsync(int unpaidRequestId, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidRequestDb>> GetAllUnpaidRequestAsync(int unpaidId, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidRequestDb>> GetAllUnpaidRequestAsync(CancellationToken cancellationToken);
        Task<int> UpdateUnpaidRequestAsync(int unpaidRequestId, Notification notification, Status status, string statusAdditionalInfo, CancellationToken cancellationToken);
    }
}