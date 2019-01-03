using System.Collections.Generic;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidRequestOperations
    {
        Task<int> AddUnpaidRequestAsync(IEnumerable<UnpaidRequest> unpaidRequests);
        Task<UnpaidRequest> GetSingleUnpaidRequestAsync(int unpaidRequestId);
        Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync(int unpaidId);
        Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync();
    }
}
