using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager.Tests
{
    public class PushNotificationManagerTests
    {
        [Test]
        public async Task SendAsync_GIVEN_Valid_Input_WebTokenSuccess_PushNotificationSuccess_True_RETURNS_Valid_NotificationResponse_OK()
        {
            // Arrange.
            var pushNotificationClient = Substitute.For<IPushNotificationClient>();

            var accessToken = "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";
            var tokenType = "bearer";

            pushNotificationClient.GetAccessTokenAsync(CancellationToken.None)
                .Returns(Task.FromResult(new PushNotificationWebTokenResponse
                {
                    AccessToken = accessToken,
                    TokenType = tokenType,
                    ExpiresIn = 315359999,
                    Issued = DateTimeOffset.Parse("Tue, 08 Jan 2019 07:46:40 GMT").UtcDateTime,
                    Expires = DateTimeOffset.Parse("Fri, 05 Jan 2029 07:46:40 GMT").UtcDateTime
                }));

            var idNumber = "9009165023080";
            var title = "Test Title";
            var message = "Test Message";

            pushNotificationClient
                .SendPushNotification(accessToken, tokenType, Arg.Any<PushNotificationRequest>(),
                    CancellationToken.None).Returns(Task.FromResult(new PushNotificationResponse
                {
                    Success = true,
                    Message = string.Empty
                }));

            var pushNotificationManager = new PushNotificationManager(pushNotificationClient);

            // Act.
            var actual = await pushNotificationManager.SendAsync(title, message, idNumber, CancellationToken.None);

            // Assert.
            Assert.AreEqual(HttpStatusCode.Accepted, actual.StatusCode);
            Assert.AreEqual(string.Empty, actual.AdditionalErrorMessage);
        }

        [Test]
        public async Task SendAsync_GIVEN_Valid_Input_WebTokenSuccess_PushNotificationSuccess_False_RETURNS_Valid_NotificationResponse_BadRequest()
        {
            // Arrange.
            var pushNotificationClient = Substitute.For<IPushNotificationClient>();

            var accessToken = "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";
            var tokenType = "bearer";

            pushNotificationClient.GetAccessTokenAsync(CancellationToken.None)
                .Returns(Task.FromResult(new PushNotificationWebTokenResponse
                {
                    AccessToken = accessToken,
                    TokenType = tokenType,
                    ExpiresIn = 315359999,
                    Issued = DateTimeOffset.Parse("Tue, 08 Jan 2019 07:46:40 GMT").UtcDateTime,
                    Expires = DateTimeOffset.Parse("Fri, 05 Jan 2029 07:46:40 GMT").UtcDateTime
                }));

            var idNumber = "9009165023080";
            var title = "Test Title";
            var message = "Test Message";

            pushNotificationClient
                .SendPushNotification(accessToken, tokenType, Arg.Any<PushNotificationRequest>(),
                    CancellationToken.None).Returns(Task.FromResult(new PushNotificationResponse
                    {
                        Success = false,
                        Message = "Could not find user with ID Number: 9009165023080"
                    }));

            var pushNotificationManager = new PushNotificationManager(pushNotificationClient);

            // Act.
            var actual = await pushNotificationManager.SendAsync(title, message, idNumber, CancellationToken.None);

            // Assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual("Could not find user with ID Number: 9009165023080", actual.AdditionalErrorMessage);
        }

        [Test]
        public async Task SendAsync_GIVEN_Valid_Input_WebTokenFail_RETURNS_Valid_NotificationResponse_ServiceUnavailable()
        {
            // Arrange.
            var pushNotificationClient = Substitute.For<IPushNotificationClient>();

            pushNotificationClient.GetAccessTokenAsync(CancellationToken.None)
                .Returns(Task.FromResult<PushNotificationWebTokenResponse>(null));

            var idNumber = "9009165023080";
            var title = "Test Title";
            var message = "Test Message";            

            var pushNotificationManager = new PushNotificationManager(pushNotificationClient);

            // Act.
            var actual = await pushNotificationManager.SendAsync(title, message, idNumber, CancellationToken.None);

            // Assert.
            Assert.AreEqual(HttpStatusCode.ServiceUnavailable, actual.StatusCode);
            Assert.AreEqual("Error getting a WebToken.", actual.AdditionalErrorMessage);
        }

        [Test]
        public async Task SendAsync_GIVEN_Valid_Input_WebTokenSuccess_PushNotificationFail_RETURNS_Valid_NotificationResponse_ServiceUnavailable()
        {
            // Arrange.
            var pushNotificationClient = Substitute.For<IPushNotificationClient>();

            var accessToken = "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";
            var tokenType = "bearer";

            pushNotificationClient.GetAccessTokenAsync(CancellationToken.None)
                .Returns(Task.FromResult(new PushNotificationWebTokenResponse
                {
                    AccessToken = accessToken,
                    TokenType = tokenType,
                    ExpiresIn = 315359999,
                    Issued = DateTimeOffset.Parse("Tue, 08 Jan 2019 07:46:40 GMT").UtcDateTime,
                    Expires = DateTimeOffset.Parse("Fri, 05 Jan 2029 07:46:40 GMT").UtcDateTime
                }));

            var idNumber = "9009165023080";
            var title = "Test Title";
            var message = "Test Message";

            pushNotificationClient
                .SendPushNotification(accessToken, tokenType, Arg.Any<PushNotificationRequest>(),
                    CancellationToken.None).Returns(Task.FromResult<PushNotificationResponse>(null));

            var pushNotificationManager = new PushNotificationManager(pushNotificationClient);

            // Act.
            var actual = await pushNotificationManager.SendAsync(title, message, idNumber, CancellationToken.None);

            // Assert.
            Assert.AreEqual(HttpStatusCode.ServiceUnavailable, actual.StatusCode);
            Assert.AreEqual("Error creating a Push Notification request.", actual.AdditionalErrorMessage);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task SendAsync_GIVEN_Invalid_Title_Input_RETURNS_Valid_NotificationResponse_BadRequest(string input)
        {
            // Arrange.
            var pushNotificationClient = Substitute.For<IPushNotificationClient>();

            var idNumber = "9009165023080";
            var title = input;
            var message = "Test Message";

            var pushNotificationManager = new PushNotificationManager(pushNotificationClient);

            // Act.
            var actual = await pushNotificationManager.SendAsync(title, message, idNumber, CancellationToken.None);

            // Assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual("Title is null or empty.", actual.AdditionalErrorMessage);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task SendAsync_GIVEN_Invalid_Message_Input_RETURNS_Valid_NotificationResponse_BadRequest(string input)
        {
            // Arrange.
            var pushNotificationClient = Substitute.For<IPushNotificationClient>();

            var idNumber = "9009165023080";
            var title = "Test Title";
            var message = input;

            var pushNotificationManager = new PushNotificationManager(pushNotificationClient);

            // Act.
            var actual = await pushNotificationManager.SendAsync(title, message, idNumber, CancellationToken.None);

            // Assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual("Message is null or empty.", actual.AdditionalErrorMessage);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task SendAsync_GIVEN_Invalid_IdNumber_Input_RETURNS_Valid_NotificationResponse_BadRequest(string input)
        {
            // Arrange.
            var pushNotificationClient = Substitute.For<IPushNotificationClient>();

            var idNumber = input;
            var title = "Test Title";
            var message = "Test Message";      

            var pushNotificationManager = new PushNotificationManager(pushNotificationClient);

            // Act.
            var actual = await pushNotificationManager.SendAsync(title, message, idNumber, CancellationToken.None);

            // Assert.
            Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual("IdNumber is null or empty.", actual.AdditionalErrorMessage);
        }
    }
}
