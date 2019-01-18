using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidModels;

namespace DataManager.Interfaces
{
    public interface IUnpaidBatchStorageOperations
    {
        Task<int> AddUnpaidBatchAsync(TbUnpaidBatch unpaidBatch, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidBatch>> GetUnpaidBatchesAsync(int statusId, CancellationToken cancellationToken);
    }
}
