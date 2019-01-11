using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidResponseStorageOperations
    {
        Task<int> AddUnpaidResponseAsync(IEnumerable<TbUnpaidResponse> unpaidResponses, CancellationToken cancellationToken);
        Task<TbUnpaidResponse> GetSingleUnpaidResponseAsync(int unpaidResponseId, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidResponse>> GetAllUnpaidResponseAsync(int unpaidRequestId, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidResponse>> GetAllUnpaidResponseAsync(CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidResponse>> GetAllUnpaidResponseJoinUnpaidRequest(CancellationToken cancellationToken);
    }
}
