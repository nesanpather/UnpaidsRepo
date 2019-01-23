using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidStorageOperations
    {
        Task<int> AddUnpaidAsync(IEnumerable<TbUnpaid> unpaids, CancellationToken cancellationToken);
        Task<TbUnpaid> GetSingleUnpaidAsync(int unpaidId, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaid>> GetAllUnpaidAsync(string batchKey, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaid>> GetAllUnpaidAsync(CancellationToken cancellationToken);
    }
}
