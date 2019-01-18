using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidEngineHandler
    {
        Task<UnpaidOutput> HandleUnpaidAsync(IEnumerable<UnpaidInput> unpaids, string idempotencyKey, CancellationToken cancellationToken);
        Task<bool> HandleUnpaidRequestAsync(IEnumerable<TbUnpaid> unpaids, CancellationToken cancellationToken);
        Task<IEnumerable<UnpaidResponseOutput>> HandleUnpaidResponseAsync(IEnumerable<UnpaidResponseInput> unpaidResponseInputs, CancellationToken cancellationToken);
    }
}
