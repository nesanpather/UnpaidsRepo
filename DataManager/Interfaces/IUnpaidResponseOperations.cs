using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidResponseOperations
    {
        Task<int> AddUnpaidResponseAsync(IEnumerable<UnpaidResponse> unpaidResponses, CancellationToken cancellationToken);
        Task<UnpaidResponse> GetSingleUnpaidResponseAsync(int unpaidResponseId, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync(int unpaidRequestId, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync(CancellationToken cancellationToken);
    }
}
