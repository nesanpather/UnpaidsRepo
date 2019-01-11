using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidClient
    {
        Task<int> AddUnpaidAsync(IEnumerable<UnpaidInput> unpaids, string idempotencyKey, CancellationToken cancellationToken);

        Task<IEnumerable<TbUnpaid>> GetUnpaidsByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken);
    }
}
