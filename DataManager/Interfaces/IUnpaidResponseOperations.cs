using System.Collections.Generic;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidResponseOperations
    {
        Task<int> AddUnpaidResponseAsync(UnpaidResponse unpaidResponse);
        Task<UnpaidResponse> GetUnpaidResponseAsync(int unpaidRequestId);
        Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync();
    }
}
