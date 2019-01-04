using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidOperations
    {
        Task<int> AddUnpaidAsync(IEnumerable<Unpaid> unpaids, CancellationToken cancellationToken);
        Task<Unpaid> GetSingleUnpaidAsync(int unpaidId, CancellationToken cancellationToken);
        Task<IEnumerable<Unpaid>> GetAllUnpaidAsync(string policyNumber, CancellationToken cancellationToken);
        Task<IEnumerable<Unpaid>> GetAllUnpaidAsync(CancellationToken cancellationToken);
    }
}
