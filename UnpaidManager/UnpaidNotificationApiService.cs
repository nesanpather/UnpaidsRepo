using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnpaidManager.Interfaces;
using Utilities.Interfaces;

namespace UnpaidManager
{
    public class UnpaidNotificationApiService: IUnpaidNotificationApiClient
    {
        private readonly IHttpClientOperations _httpClientOperations;
        private readonly ISettings _settings;

        public UnpaidNotificationApiService(IHttpClientOperations httpClientOperations, ISettings settings)
        {
            _httpClientOperations = httpClientOperations;
            _settings = settings;
        }

        public async Task<string> DelegateToNotificationApiAsync(string idempotencyKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                // Log Error.
                return null;
            }

            var url = _settings["UnpaidNotificationApi:Url"];
            if (string.IsNullOrWhiteSpace(url))
            {
                // Log Error.
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
                // Log Error.
                return null;
            }

            return responseString;
        }
    }
}
