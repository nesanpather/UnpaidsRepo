using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Utilities.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _fakeResponseString;

        public FakeHttpMessageHandler(string fakeResponseString)
        {
            _fakeResponseString = fakeResponseString;
        }

        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(_fakeResponseString, Encoding.UTF8)
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }

    public class HttpClientManagerTests
    {
        [Test]
        public async Task SendHttpRequestAsync_GIVEN_Valid_Input_RETURNS_Valid_StringNotNull()
        {
            // Arrange.
            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            var responseObject = new { Name = "Test", Surname = "Smith"};

            var fakeMsgHandler = new FakeHttpMessageHandler(JsonConvert.SerializeObject(responseObject));            
            var fakeHttpClient = new HttpClient(fakeMsgHandler);

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(info => fakeHttpClient);

            var httpClientManager = new HttpClientManager(httpClientFactory);

            var authorizationHeader = new AuthenticationHeaderValue("Basic", "sdfdsf");
            var acceptHeader = new Dictionary<string, string>{ {"Accept", "application/json" } };

            var requestContent = new { ReferenceNumber = "Test1234" };

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, acceptHeader, "http://test.co.za", requestContent, CancellationToken.None);

            // Assert.
            Assert.NotNull(actual);
            Assert.AreEqual("{\"Name\":\"Test\",\"Surname\":\"Smith\"}", actual);
        }

        [Test]
        public async Task SendHttpRequestAsync_GIVEN_Invalid_AuthenticationHeaderValue_Input_RETURNS_Valid_StringNotNull()
        {
            // Arrange.
            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            var responseObject = new { Name = "Test", Surname = "Smith" };

            var fakeMsgHandler = new FakeHttpMessageHandler(JsonConvert.SerializeObject(responseObject));
            var fakeHttpClient = new HttpClient(fakeMsgHandler);

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(info => fakeHttpClient);

            var httpClientManager = new HttpClientManager(httpClientFactory);                  
            var acceptHeader = new Dictionary<string, string> { { "Accept", "application/json" } };

            var requestContent = new { ReferenceNumber = "Test1234" };

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Post, null, acceptHeader, "http://test.co.za", requestContent, CancellationToken.None);

            // Assert.
            Assert.NotNull(actual);
            Assert.AreEqual("{\"Name\":\"Test\",\"Surname\":\"Smith\"}", actual);
        }

        [Test]
        public async Task SendHttpRequestAsync_GIVEN_Invalid_HttpMethod_Input_RETURNS_Null()
        {
            // Arrange.
            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            var responseObject = new { Name = "Test", Surname = "Smith" };

            var fakeMsgHandler = new FakeHttpMessageHandler(JsonConvert.SerializeObject(responseObject));
            var fakeHttpClient = new HttpClient(fakeMsgHandler);

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(info => fakeHttpClient);

            var httpClientManager = new HttpClientManager(httpClientFactory);
            var authorizationHeader = new AuthenticationHeaderValue("Basic", "sdfdsf");
            var acceptHeader = new Dictionary<string, string> { { "Accept", "application/json" } };

            var requestContent = new { ReferenceNumber = "Test1234" };

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(null, authorizationHeader, acceptHeader, "http://test.co.za", requestContent, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task SendHttpRequestAsync_GIVEN_Invalid_Url_Input_RETURNS_Null(string url)
        {
            // Arrange.
            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            var responseObject = new { Name = "Test", Surname = "Smith" };

            var fakeMsgHandler = new FakeHttpMessageHandler(JsonConvert.SerializeObject(responseObject));
            var fakeHttpClient = new HttpClient(fakeMsgHandler);

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(info => fakeHttpClient);

            var httpClientManager = new HttpClientManager(httpClientFactory);
            var authorizationHeader = new AuthenticationHeaderValue("Basic", "sdfdsf");
            var acceptHeader = new Dictionary<string, string> { { "Accept", "application/json" } };

            var requestContent = new { ReferenceNumber = "Test1234" };

            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, acceptHeader, url, requestContent, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task SendHttpRequestAsync_GIVEN_Invalid_Content_Input_RETURNS_Null()
        {
            // Arrange.
            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            var responseObject = new { Name = "Test", Surname = "Smith" };

            var fakeMsgHandler = new FakeHttpMessageHandler(JsonConvert.SerializeObject(responseObject));
            var fakeHttpClient = new HttpClient(fakeMsgHandler);

            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(info => fakeHttpClient);

            var httpClientManager = new HttpClientManager(httpClientFactory);
            var authorizationHeader = new AuthenticationHeaderValue("Basic", "sdfdsf");
            var acceptHeader = new Dictionary<string, string> { { "Accept", "application/json" } };
            
            // Act.
            var actual = await httpClientManager.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, acceptHeader, "http://www.test.com", null, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }
    }
}
