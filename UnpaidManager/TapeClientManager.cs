using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnpaidManager.Interfaces;
using UnpaidModels;
using Utilities;
using Utilities.Interfaces;

namespace UnpaidManager
{
    public class TapeClientManager : IEnterpriseServerOperations
    {
        private readonly IHttpClientOperations _httpClientOperations;
        private readonly ISettings _settings;

        public TapeClientManager(IHttpClientOperations httpClientOperations, ISettings settings)
        {
            _httpClientOperations = httpClientOperations;
            _settings = settings;
        }

        public async Task<ClientDetails> GetClientDetailsAsync(string policyNumber, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
            {
                // Log Error.
                return null;
            }

            var tapeUrl = _settings["Tape:Url"];
            if (string.IsNullOrWhiteSpace(tapeUrl))
            {
                // Log Error.
                return null;
            }

            var tapeEnvironment = _settings["Tape:Environment"];
            if (string.IsNullOrWhiteSpace(tapeEnvironment))
            {
                // Log Error.
                return null;
            }

            var tapeUsername = _settings["Tape:Username"];
            if (string.IsNullOrWhiteSpace(tapeUsername))
            {
                // Log Error.
                return null;
            }

            var tapePassword = _settings["Tape:Password"];
            if (string.IsNullOrWhiteSpace(tapePassword))
            {
                // Log Error.
                return null;
            }

            const string tapeService = "Person";
            const string tapeMethod = "ws_getclient";

            var url = $"{Uri.EscapeDataString(tapeUrl)}/{Uri.EscapeDataString(tapeEnvironment)}/{tapeService}/{tapeMethod}";

            var authHeaderParameter = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{tapeUsername}:{tapePassword}"));
            var authorizationHeader = new AuthenticationHeaderValue("Basic", authHeaderParameter);

            var headerDictionary = new Dictionary<string, string> { { "Accept", "application/json" } };

            // TODO: TTSEncryption?
            //var referenceNumber = $"{policyNumber}|{TTSCryptography.Encrypt(policyNumber)}";
            var referenceNumber = $"{policyNumber}";

            var serializedRequest = JsonConvert.SerializeObject(new {referenceNumber});
            var httpContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var getClientDetailResult = await _httpClientOperations.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, headerDictionary, url, httpContent, cancellationToken);

            if (getClientDetailResult == null)
            {
                // Log Error.
                return null;
            }

            return JsonConvert.DeserializeObject<ClientDetails>(getClientDetailResult);
        }
    }
}
