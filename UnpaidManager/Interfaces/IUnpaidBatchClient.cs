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
        Task<int> AddUnpaidBatchAsync(string idempotencyId, Status status, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidBatch>> GetUnpaidBatchByStatusAsync(Status status, CancellationToken cancellationToken);
    }
}
