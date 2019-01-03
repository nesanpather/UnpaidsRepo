using System.Collections.Generic;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidResponseOperations
    {
        Task<int> AddUnpaidResponseAsync(IEnumerable<UnpaidResponse> unpaidResponses);
        Task<UnpaidResponse> GetSingleUnpaidResponseAsync(int unpaidResponseId);
        Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync(int unpaidRequestId);
        Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync();
    }
}
