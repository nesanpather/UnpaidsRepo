using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Interfaces;

namespace Utilities
{
    public class HttpClientManager: IHttpClientOperations
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public HttpClientManager(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> SendHttpRequestAsync(HttpMethod httpMethod, AuthenticationHeaderValue authenticationHeaderValue, Dictionary<string, string> headerDictionary, string url, HttpContent httpContent, CancellationToken cancellationToken)
        {
            if (httpMethod == null)
            {
                // Log error.
                ConsoleLogger.SingleInstance.LogError("HttpMethod is null or empty.");
                return null;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                // Log Error. Invalid URI: The URI is empty.
                return null;
            }

            if (httpContent == null)
            {
                // Log error.
                return null;
            }

            var uri = new Uri(url);
            
            var request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Authorization = authenticationHeaderValue;
            
            foreach (var header in headerDictionary)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            request.Content = httpContent;

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}
