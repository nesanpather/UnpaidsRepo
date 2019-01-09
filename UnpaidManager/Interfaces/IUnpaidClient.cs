using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidClient
    {
        Task<int> AddUnpaidAsync(IEnumerable<Unpaid> unpaids, string idempotencyKey, CancellationToken cancellationToken);

        Task<IEnumerable<UnpaidDb>> GetUnpaidsByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken);
    }
}
