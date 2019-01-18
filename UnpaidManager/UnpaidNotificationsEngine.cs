using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using UnpaidManager.Interfaces;

namespace UnpaidManager
{
    public class UnpaidNotificationsEngine: IUnpaidNotificationsEngineHandler
    {
        private readonly IUnpaidEngineHandler _unpaidEngineHandler;
        private readonly IUnpaidClient _unpaidClient;

        public UnpaidNotificationsEngine(IUnpaidEngineHandler unpaidEngineHandler, IUnpaidClient unpaidClient)
        {
            _unpaidEngineHandler = unpaidEngineHandler;
            _unpaidClient = unpaidClient;
        }

        public async Task HandleUnpaidNotificationsAsync(string idempotencyKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                // Log Error.
                return;                
            }

            // Get unpaids to work with by idempotencyKey
            var unpaidsByIdempotencyResult = await _unpaidClient.GetUnpaidsByIdempotencyKeyAsync(idempotencyKey, cancellationToken);
            if (unpaidsByIdempotencyResult == null)
            {
                // Log Error.
                return;
            }

            var byIdempotencyResultList = unpaidsByIdempotencyResult.ToList();

            if (!byIdempotencyResultList.Any())
            {
                // Log Error.
                return;
            }

            BackgroundJob.Enqueue(() => _unpaidEngineHandler.HandleUnpaidRequestAsync(byIdempotencyResultList, cancellationToken));
        }
    }
}
