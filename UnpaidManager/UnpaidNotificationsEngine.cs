using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager
{
    public class UnpaidNotificationsEngine: IUnpaidNotificationsEngineHandler
    {
        private readonly IUnpaidEngineHandler _unpaidEngineHandler;
        private readonly IUnpaidClient _unpaidClient;
        private readonly ILogger<UnpaidNotificationsEngine> _logger;

        public UnpaidNotificationsEngine(IUnpaidEngineHandler unpaidEngineHandler, IUnpaidClient unpaidClient, ILogger<UnpaidNotificationsEngine> logger)
        {
            _unpaidEngineHandler = unpaidEngineHandler;
            _unpaidClient = unpaidClient;
            _logger = logger;
        }

        public async Task HandleUnpaidNotificationsAsync(string idempotencyKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                _logger.LogWarning((int)LoggingEvents.GetItem, "UnpaidNotificationsEngine.HandleUnpaidNotificationsAsync - idempotencyKey is null or empty", new { BatchKey = idempotencyKey });
                return;                
            }

            // Get unpaids to work with by idempotencyKey
            var unpaidsByIdempotencyResult = await _unpaidClient.GetUnpaidsByIdempotencyKeyAsync(idempotencyKey, cancellationToken);
            if (unpaidsByIdempotencyResult == null)
            {
                _logger.LogWarning((int)LoggingEvents.GetItem, "UnpaidNotificationsEngine.HandleUnpaidNotificationsAsync - _unpaidClient.GetUnpaidsByIdempotencyKeyAsync returned null", new { BatchKey = idempotencyKey });
                return;
            }

            var byIdempotencyResultList = unpaidsByIdempotencyResult.ToList();

            if (!byIdempotencyResultList.Any())
            {
                _logger.LogWarning((int)LoggingEvents.GetItem, "UnpaidNotificationsEngine.HandleUnpaidNotificationsAsync - _unpaidClient.GetUnpaidsByIdempotencyKeyAsync returned empty results", new { BatchKey = idempotencyKey });
                return;
            }

            BackgroundJob.Enqueue(() => _unpaidEngineHandler.HandleUnpaidRequestAsync(byIdempotencyResultList, idempotencyKey, cancellationToken));
        }
    }
}
