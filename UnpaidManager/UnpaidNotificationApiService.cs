using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnpaidManager.Interfaces;
using UnpaidModels;
using Utilities.Interfaces;

namespace UnpaidManager
{
    public class UnpaidNotificationApiService: IUnpaidNotificationApiClient
    {
        private readonly IHttpClientOperations _httpClientOperations;
        private readonly ISettings _settings;
        private readonly ILogger<UnpaidNotificationApiService> _logger;

        public UnpaidNotificationApiService(IHttpClientOperations httpClientOperations, ISettings settings, ILogger<UnpaidNotificationApiService> logger)
        {
            _httpClientOperations = httpClientOperations;
            _settings = settings;
            _logger = logger;
        }

        public async Task<string> DelegateToNotificationApiAsync(string idempotencyKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidNotificationApiService.DelegateToNotificationApiAsync - idempotencyKey is null or empty", new { BatchKey = idempotencyKey });
                return null;
            }

            var url = _settings["UnpaidNotificationApi:Url"];
            if (string.IsNullOrWhiteSpace(url))
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "UnpaidNotificationApiService.DelegateToNotificationApiAsync - _settings['UnpaidNotificationApi: Url'] is null or empty", new { BatchKey = idempotencyKey });
                return null;
            }

            url = $"{url}/{idempotencyKey}";

            var headers = new Dictionary<string, string>
            {
                {"Accept", "application/json"}
            };

            var httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            var responseString = await _httpClientOperations.SendHttpRequestAsync(HttpMethod.Get, null, headers, url, httpContent, cancellationToken);

            if (string.IsNullOrWhiteSpace(responseString))
            {
                _logger.LogError((int)LoggingEvents.ExternalCall, "UnpaidNotificationApiService.DelegateToNotificationApiAsync - _httpClientOperations.SendHttpRequestAsync response is null or empty.", new { BatchKey = idempotencyKey });
                return null;
            }

            return responseString;
        }
    }
}
