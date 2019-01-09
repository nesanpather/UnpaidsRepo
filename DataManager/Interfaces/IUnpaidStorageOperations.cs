using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidStorageOperations
    {
        Task<int> AddUnpaidAsync(IEnumerable<UnpaidDb> unpaids, CancellationToken cancellationToken);
        Task<UnpaidDb> GetSingleUnpaidAsync(int unpaidId, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidDb>> GetAllUnpaidAsync(string idempotencyKey, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidDb>> GetAllUnpaidAsync(CancellationToken cancellationToken);
    }
}
