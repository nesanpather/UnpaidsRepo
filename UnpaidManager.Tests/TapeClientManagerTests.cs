using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using Utilities.Interfaces;

namespace UnpaidManager.Tests
{
    public class TapeClientManagerTests
    {
        [Test]
        public async Task GetClientDetailsAsync_GIVEN_Valid_Input_Valid_Settings_RETURNS_Valid_ClientDetails()
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["Tape.Url"] = "http://tape.telesure.co.za/";
            settings["Tape.Environment"] = "DEN0114";
            settings["Tape.Username"] = "upstream";
            settings["Tape.Password"] = "uPstream23@";

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            httpClientOperations.SendHttpRequestAsync(HttpMethod.Post, Arg.Any<AuthenticationHeaderValue>(),
                    Arg.Any<Dictionary<string, string>>(), "http://tape.telesure.co.za/DEN0114/Person/ws_getclient", Arg.Any<HttpContent>(), CancellationToken.None)
                .Returns(Task.FromResult(""));

            var tapeClientManager = new TapeClientManager(httpClientOperations, settings);

            // Act.
            var actual = await tapeClientManager.GetClientDetailsAsync("777291226", CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
        }
    }
}
