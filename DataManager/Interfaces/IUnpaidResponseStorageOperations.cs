using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidResponseStorageOperations
    {
        Task<int> AddUnpaidResponseAsync(IEnumerable<UnpaidResponseDb> unpaidResponses, CancellationToken cancellationToken);
        Task<UnpaidResponseDb> GetSingleUnpaidResponseAsync(int unpaidResponseId, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidResponseDb>> GetAllUnpaidResponseAsync(int unpaidRequestId, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidResponseDb>> GetAllUnpaidResponseAsync(CancellationToken cancellationToken);
    }
}
