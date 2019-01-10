using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnpaidManager.Interfaces;
using UnpaidModels;
using Utilities.Interfaces;

namespace UnpaidManager
{
    public class PushNotificationService: IPushNotificationClient
    {
        private readonly IHttpClientOperations _httpClientOperations;
        private readonly ISettings _settings;

        public PushNotificationService(IHttpClientOperations httpClientOperations, ISettings settings)
        {
            _httpClientOperations = httpClientOperations;
            _settings = settings;
        }

        public async Task<PushNotificationWebTokenResponse> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var url = _settings["PushNotification:WebTokenUrl"]; // https://drivewithdialstable.retrotest.co.za/api/v1/token
            if (string.IsNullOrWhiteSpace(url))
            {
                // Log Error.
                return null;
            }

            var clientSecret = _settings["PushNotification:WebTokenClientSecret"]; // bNvrT3EsEnPwDvwy46wVyev7DHR4f86e32LVZBJk6ej
            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                // Log Error.
                return null;
            }

            var grantType = _settings["PushNotification:WebTokenGrantType"]; // client_credentials
            var clientId = _settings["PushNotification:WebTokenClientId"]; // push-user

            var headers = new Dictionary<string, string>();

            var keyValueContent = new Dictionary<string, string>
            {
                {"grant_type", grantType},
                {"client_id", clientId},
                {"client_secret", clientSecret}
            };

            var httpContent = new FormUrlEncodedContent(keyValueContent);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var responseString = await _httpClientOperations.SendHttpRequestAsync(HttpMethod.Post, null, headers, url, httpContent, cancellationToken);

            if (string.IsNullOrWhiteSpace(responseString))
            {
                // Log Error.
                return null;
            }

            return JsonConvert.DeserializeObject<PushNotificationWebTokenResponse>(responseString);
        }

        public async Task<PushNotificationResponse> SendPushNotification(string accessToken, string tokenType, PushNotificationRequest pushNotificationRequest, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                // Log Error.
                return null;
            }

            if (pushNotificationRequest == null)
            {
                // Log Error.
                return null;
            }

            if (string.IsNullOrWhiteSpace(pushNotificationRequest.IdNumber))
            {
                // Log Error.
                return null;
            }

            if (string.IsNullOrWhiteSpace(pushNotificationRequest.Title))
            {
                // Log Error.
                return null;
            }

            var url = _settings["PushNotification:Url"]; // https://drivewithdialstable.retrotest.co.za/api/v1/profile/send/push/for/idnumber

            if (string.IsNullOrWhiteSpace(url))
            {
                // Log Error.
                return null;
            }

            var headers = new Dictionary<string, string>
            {                
                {"Accept-Charset", "UTF-8"},
                {"Accept", "application/json"}
            };

            var authorizationHeader = new AuthenticationHeaderValue(tokenType.Trim(), accessToken);

            var serializedRequest = JsonConvert.SerializeObject(pushNotificationRequest);
            var httpContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var responseString = await _httpClientOperations.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, headers, url, httpContent, cancellationToken);

            if (string.IsNullOrWhiteSpace(responseString))
            {
                // Log Error.
                return null;
            }

            return JsonConvert.DeserializeObject<PushNotificationResponse>(responseString);
        }
    }
}
