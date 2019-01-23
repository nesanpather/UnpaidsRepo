using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidBatchClient
    {
        Task<int> AddUnpaidBatchAsync(string batchKey, Status status, string userName, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidBatch>> GetUnpaidBatchByBatchKeyAsync(string batchKey, CancellationToken cancellationToken);
        Task<int> UpdateUnpaidBatchAsync(string batchKey, Status status, DateTime dateModified, CancellationToken cancellationToken);
    }
}
