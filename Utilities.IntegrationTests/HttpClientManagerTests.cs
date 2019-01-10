using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Text;

namespace Utilities.IntegrationTests
{
    public class HttpClientManagerTests
    {
        [SetUp]
        public void Setup()
        {          
        }

        [Test]
        public async Task SendHttpRequestAsync_GIVEN_Valid_Input_HttpMethod_Get_RETURNS_Valid_StringNotNull()
        {
            // Arrange.   
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var httpClientManager = new HttpClientManager(httpClientFactory);

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(info => new HttpClient());
            var authorizationHeader = new AuthenticationHeaderValue("Basic", "sdfdsf");
            var acceptHeader = new Dictionary<string, string> { { "Accept", "application/json" }, { "User-Agent", "HttpClientManagerIntegrationTests" } };

            var requestContent = new { ReferenceNumber = "Test1234" };
            var serializedRequest = JsonConvert.SerializeObject(requestContent);
            var httpContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Get, authorizationHeader, acceptHeader, "https://httpbin.org/get", httpContent, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task SendHttpRequestAsync_GIVEN_Valid_Input_HttpMethod_Post_RETURNS_Valid_StringNotNull()
        {
            // Arrange.   
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var httpClientManager = new HttpClientManager(httpClientFactory);

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(info => new HttpClient());
            var authorizationHeader = new AuthenticationHeaderValue("Basic", "sdfdsf");
            var acceptHeader = new Dictionary<string, string> { { "Accept", "application/json" }, { "User-Agent", "HttpClientManagerIntegrationTests" } };

            var requestContent = new { ReferenceNumber = "Test1234" };
            var serializedRequest = JsonConvert.SerializeObject(requestContent);
            var httpContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, acceptHeader, "https://httpbin.org/post", httpContent, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task SendHttpRequestAsync_GIVEN_Valid_Input_HttpMethod_Post_FormUrlEncodedContent_RETURNS_Valid_StringNotNull()
        {
            // Arrange.   
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var httpClientManager = new HttpClientManager(httpClientFactory);

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(info => new HttpClient());
            var authorizationHeader = new AuthenticationHeaderValue("Basic", "sdfdsf");
            var acceptHeader = new Dictionary<string, string> { { "Accept", "application/json" }, { "User-Agent", "HttpClientManagerIntegrationTests" } };

            var keyValueContent = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"client_id", "push-user"},
                {"client_secret", "SECRET"}
            };

            var httpContent = new FormUrlEncodedContent(keyValueContent);

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, acceptHeader, "https://httpbin.org/post", httpContent, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task SendHttpRequestAsync_GIVEN_Valid_Input_HttpMethod_Post_GetAccessTokenAsync_RETURNS_Valid_StringNotNull()
        {
            // Arrange.   
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var httpClientManager = new HttpClientManager(httpClientFactory);

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(info => new HttpClient());

            var headers = new Dictionary<string, string>();
            var keyValueContent = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"client_id", "push-user"},
                {"client_secret", "bNvrT3EsEnPwDvwy46wVyev7DHR4f86e32LVZBJk6ej"}
            };

            var httpContent = new FormUrlEncodedContent(keyValueContent);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Post, null, headers, "https://drivewithdialstable.retrotest.co.za/api/v1/token", httpContent, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
        }
    }
}