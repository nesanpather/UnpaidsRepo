using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using Utilities.Interfaces;
using UnpaidModels;

namespace UnpaidManager.Tests
{
    public class PushNotificationServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAccessTokenAsync_GIVEN_Valid_Settings_RETURNS_Valid_PushNotificationWebTokenResponse()
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:WebTokenUrl"] = "https://www.test.com";
            settings["PushNotification:WebTokenClientSecret"] = "dgfdggdfgdgfgfdgdg";
            settings["PushNotification:WebTokenGrantType"] = "client_credentials";
            settings["PushNotification:WebTokenClientId"] = "push-user";

            var httpClientOperations = Substitute.For<IHttpClientOperations>();

            httpClientOperations.SendHttpRequestAsync(HttpMethod.Post, null, Arg.Any<Dictionary<string, string>>(),
                "https://www.test.com", Arg.Any<HttpContent>(), CancellationToken.None).Returns(
                Task.FromResult(
                    "{\r\n    \"access_token\": \"nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM\",\r\n    \"token_type\": \"bearer\",\r\n    \"expires_in\": 315359999,\r\n    \".issued\": \"Tue, 08 Jan 2019 07:46:40 GMT\",\r\n    \".expires\": \"Fri, 05 Jan 2029 07:46:40 GMT\"\r\n}")
            );

            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.GetAccessTokenAsync(CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            Assert.AreEqual(
                "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM",
                actual.AccessToken);
            Assert.AreEqual("bearer",actual.TokenType);
            Assert.AreEqual(315359999, actual.ExpiresIn);
            Assert.AreEqual(DateTimeOffset.Parse("Tue, 08 Jan 2019 07:46:40 GMT").UtcDateTime, actual.Issued);
            Assert.AreEqual(DateTimeOffset.Parse("Fri, 05 Jan 2029 07:46:40 GMT").UtcDateTime, actual.Expires);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task GetAccessTokenAsync_GIVEN_Invalid_Settings_Url_RETURNS_Null(string url)
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:WebTokenUrl"] = url;
            settings["PushNotification:WebTokenClientSecret"] = "dgfdggdfgdgfgfdgdg";
            settings["PushNotification:WebTokenGrantType"] = "client_credentials";
            settings["PushNotification:WebTokenClientId"] = "push-user";

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.GetAccessTokenAsync(CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task GetAccessTokenAsync_GIVEN_Invalid_Settings_ClientSecret_RETURNS_Null(string clientSecret)
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:WebTokenUrl"] = "https://www.test.com";
            settings["PushNotification:WebTokenClientSecret"] = clientSecret;
            settings["PushNotification:WebTokenGrantType"] = "client_credentials";
            settings["PushNotification:WebTokenClientId"] = "push-user";

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.GetAccessTokenAsync(CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task GetAccessTokenAsync_GIVEN_Valid_Settings_SendHttpRequestAsync_Returns_InvalidResponse_RETURNS_Null(string response)
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:WebTokenUrl"] = "https://www.test.com";
            settings["PushNotification:WebTokenClientSecret"] = "dgfdggdfgdgfgfdgdg";
            settings["PushNotification:WebTokenGrantType"] = "client_credentials";
            settings["PushNotification:WebTokenClientId"] = "push-user";

            var httpClientOperations = Substitute.For<IHttpClientOperations>();

            httpClientOperations.SendHttpRequestAsync(HttpMethod.Post, null, Arg.Any<Dictionary<string, string>>(),
                "https://www.test.com", Arg.Any<HttpContent>(), CancellationToken.None).Returns(Task.FromResult(response));

            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.GetAccessTokenAsync(CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task SendPushNotification_GIVEN_Valid_Inputs_RETURNS_Valid_PushNotificationResponse_SuccessTrue()
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:Url"] = "https://www.test.com";

            var tokenType = "bearer";
            var accessToken =
                "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";
            var authorizationHeader = new AuthenticationHeaderValue(tokenType.Trim(), accessToken);

            var pushNotificationRequest = new PushNotificationRequest
            {
                IdNumber = "9009165023080",
                Title = "Test Title",
                Message = "Test Message"
            };

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            httpClientOperations.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, Arg.Any<Dictionary<string, string>>(),
                "https://www.test.com", Arg.Any<HttpContent>(), CancellationToken.None).Returns(Task.FromResult("{\r\n    \"success\": true,\r\n    \"message\": \"\"\r\n}"));

            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.SendPushNotification(accessToken, tokenType, pushNotificationRequest, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            Assert.AreEqual(true, actual.Success);
            Assert.AreEqual("", actual.Message);
        }

        [Test]
        public async Task SendPushNotification_GIVEN_Valid_Inputs_RETURNS_Valid_PushNotificationResponse_SuccessFalse()
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:Url"] = "https://www.test.com";

            var tokenType = "bearer";
            var accessToken =
                "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";
            var authorizationHeader = new AuthenticationHeaderValue(tokenType.Trim(), accessToken);
      
            var pushNotificationRequest = new PushNotificationRequest
            {
                IdNumber = "9009165023080",
                Title = "Test Title",
                Message = "Test Message"
            };

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            httpClientOperations.SendHttpRequestAsync(HttpMethod.Post, authorizationHeader, Arg.Any<Dictionary<string, string>>(),
                "https://www.test.com", Arg.Any<HttpContent>(), CancellationToken.None).Returns(Task.FromResult("{\r\n    \"success\": false,\r\n    \"message\": \"Could not find user with ID Number: 9009165023080\"\r\n}"));

            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.SendPushNotification(accessToken, tokenType, pushNotificationRequest, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            Assert.AreEqual(false, actual.Success);
            Assert.AreEqual("Could not find user with ID Number: 9009165023080", actual.Message);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task SendPushNotification_GIVEN_Invalid_Settings_Url_RETURNS_Null(string url)
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:Url"] = url;

            var tokenType = "bearer";
            var accessToken =
                "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";

            var pushNotificationRequest = new PushNotificationRequest
            {
                IdNumber = "9009165023080",
                Title = "Test Title",
                Message = "Test Message"
            };

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.SendPushNotification(accessToken, tokenType, pushNotificationRequest, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task SendPushNotification_GIVEN_Invalid_AccessToken_RETURNS_Null(string accessToken)
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:Url"] = "https://www.test.com";

            var tokenType = "bearer";
            var pushNotificationRequest = new PushNotificationRequest
            {
                IdNumber = "9009165023080",
                Title = "Test Title",
                Message = "Test Message"
            };

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.SendPushNotification(accessToken, tokenType, pushNotificationRequest, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task SendPushNotification_GIVEN_PushNotificationRequest_Null_RETURNS_Null()
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:Url"] = "https://www.test.com";

            var tokenType = "bearer";
            var accessToken =
                "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";    

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.SendPushNotification(accessToken, tokenType, null, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task SendPushNotification_GIVEN_Invalid_IdNumber_RETURNS_Null(string idNumber)
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:Url"] = "https://www.test.com";

            var tokenType = "bearer";
            var accessToken =
                "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";

            var pushNotificationRequest = new PushNotificationRequest
            {
                IdNumber = idNumber,
                Title = "Test Title",
                Message = "Test Message"
            };

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.SendPushNotification(accessToken, tokenType, pushNotificationRequest, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task SendPushNotification_GIVEN_Invalid_Title_RETURNS_Null(string title)
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:Url"] = "https://www.test.com";

            var tokenType = "bearer";
            var accessToken =
                "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";

            var pushNotificationRequest = new PushNotificationRequest
            {
                IdNumber = "9009165023080",
                Title = title,
                Message = "Test Message"
            };

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.SendPushNotification(accessToken, tokenType, pushNotificationRequest, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task SendPushNotification_GIVEN_Invalid_Message_RETURNS_Null(string message)
        {
            // Arrange.
            var settings = Substitute.For<ISettings>();
            settings["PushNotification:Url"] = "https://www.test.com";

            var tokenType = "bearer";
            var accessToken =
                "nV7-QR3si92u5H4rhGMRDDg-Z8b_cXpDbHekAmKFkTQjqnrtZcwURtXDVnHKUa4dnFdr29iznAkMMBWSZOeaCXAHVTYZGaaxC7hD0XHWIo5Cj8-QIiRndFrHA8GYDoqZ5sHE9-tnfbvTp6OXSI3CcTpqh95UrZCPsyRFe8SfheEvIPf018vGTgtU1_GBytePe5iINI_bw67ohkFmDXvVIZ2ym6_UXBCdXQPthorSMAiq14o8C-Mqxqc90P0pj_kNvbch2L2Ur4NWS56T_HtUpDV93ZVytR2b2e9c7USAnSAoL7P77REqBpINoKnNj62iZAsPqq22acM2nl5jv-rDoUYhxGb28P_jRq_-iiC2RG-L9_mnzv_S6saZs7m5zsPe_h_omyY1hMlQTvXwb0xYzLNHwjy_5TkAkah_-Gegutka29dKkSyAhWqlZy96-9J48SOgmtXsA1pyMbsZIDPal2ymUZiRyQe9BiiXGcQcsbneFeJbK1eKr7CIavmmpniM";

            var pushNotificationRequest = new PushNotificationRequest
            {
                IdNumber = "9009165023080",
                Title = "Test Title",
                Message = message
            };

            var httpClientOperations = Substitute.For<IHttpClientOperations>();
            var pushNotificationService = new PushNotificationService(httpClientOperations, settings);

            // Act.
            var actual = await pushNotificationService.SendPushNotification(accessToken, tokenType, pushNotificationRequest, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }
    }
}