using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

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

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Get, authorizationHeader, acceptHeader, "https://httpbin.org/get", requestContent, CancellationToken.None);

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

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, acceptHeader, "https://httpbin.org/post", requestContent, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
        }
    }
}