using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Interfaces
{
    public interface IHttpClientOperations
    {
        Task<string> SendHttpRequestAsync(HttpMethod httpMethod, AuthenticationHeaderValue authenticationHeaderValue, Dictionary<string, string> headerDictionary, string url, HttpContent httpContent, CancellationToken cancellationToken);
    }
}
