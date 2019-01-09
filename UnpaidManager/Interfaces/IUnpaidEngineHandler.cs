using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidEngineHandler
    {
        Task<IEnumerable<UnpaidOutput>> HandleUnpaidAsync(IEnumerable<Unpaid> unpaids, string idempotencyKey, CancellationToken cancellationToken);
    }
}
