using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidRequestOperations
    {
        Task<int> AddUnpaidRequestAsync(IEnumerable<UnpaidRequest> unpaidRequests, CancellationToken cancellationToken);
        Task<UnpaidRequest> GetSingleUnpaidRequestAsync(int unpaidRequestId, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync(CancellationToken cancellationToken);
    }
}
