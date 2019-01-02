using System.Collections.Generic;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidRequestOperations
    {
        Task<int> AddUnpaidRequestAsync(UnpaidRequest unpaidRequest);
        Task<UnpaidRequest> GetUnpaidRequestAsync(int unpaidId);
        Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync();
    }
}
