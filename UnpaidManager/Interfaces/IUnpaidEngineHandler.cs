using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidEngineHandler
    {
        Task<UnpaidOutput> HandleUnpaidAsync(IEnumerable<UnpaidInput> unpaids, string idempotencyKey, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidResponseOutput>> HandleUnpaidResponseAsync(IEnumerable<UnpaidResponseInput> unpaidResponseInputs, CancellationToken cancellationToken);
    }
}
