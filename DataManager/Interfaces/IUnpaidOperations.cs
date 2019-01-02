using System.Collections.Generic;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidOperations
    {
        Task<int> AddUnpaidAsync(IEnumerable<Unpaid> unpaids);
        Task<Unpaid> GetSingleUnpaidAsync(int unpaidId);
        Task<IEnumerable<Unpaid>> GetAllUnpaidAsync(string policyNumber);
        Task<IEnumerable<Unpaid>> GetAllUnpaidAsync();
    }
}
