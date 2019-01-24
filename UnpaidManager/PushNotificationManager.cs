using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using Microsoft.Extensions.Logging;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager
{
    public class PushNotificationManager : INotification
    {
        private readonly IPushNotificationClient _pushNotificationClient;
        private readonly IAccessTokenClient _accessTokenClient;
        private readonly ILogger<PushNotificationManager> _logger;

        public PushNotificationManager(IPushNotificationClient pushNotificationClient, IAccessTokenClient accessTokenClient, ILogger<PushNotificationManager> logger)
        {
            _pushNotificationClient = pushNotificationClient;
            _accessTokenClient = accessTokenClient;
            _logger = logger;
        }

        public async Task<NotificationResponse> SendAsync(string title, string message, string idNumber, string correlationId, CancellationToken cancellationToken)
        {
            var errorResponse = new NotificationResponse
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            if (string.IsNullOrWhiteSpace(title))
            {
                _logger.LogError((int) LoggingEvents.ValidationFailed, "PushNotificationManager.SendAsync - title is null or empty");
                errorResponse.AdditionalErrorMessage = "Title is null or empty.";
                return errorResponse;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "PushNotificationManager.SendAsync - message is null or empty");
                errorResponse.AdditionalErrorMessage = "Message is null or empty.";
                return errorResponse;
            }

            if (string.IsNullOrWhiteSpace(idNumber))
            {
                _logger.LogError((int)LoggingEvents.ValidationFailed, "PushNotificationManager.SendAsync - idNumber is null or empty");
                errorResponse.AdditionalErrorMessage = "IdNumber is null or empty.";
                return errorResponse;
            }

            string accessToken;
            string tokenType;

            var latestAccessTokenResult = await _accessTokenClient.GetLatestAccessTokenAsync(cancellationToken);

            if (latestAccessTokenResult != null && latestAccessTokenResult.DateExpires >= DateTime.UtcNow)
            {
                accessToken = latestAccessTokenResult.AccessToken;
                tokenType = latestAccessTokenResult.TokenType;
            }
            else
            {
                var accessTokenResult = await _pushNotificationClient.GetAccessTokenAsync(cancellationToken);

                if (accessTokenResult == null)
                {
                    _logger.LogError((int)LoggingEvents.GetItem, "PushNotificationManager.SendAsync - _pushNotificationClient.GetAccessTokenAsync returned null");
                    errorResponse.StatusCode = HttpStatusCode.ServiceUnavailable;
                    errorResponse.AdditionalErrorMessage = "Error getting a WebToken.";
                    return errorResponse;
                }

                var newAccessToken = new TbAccessToken
                {
                    AccessToken = accessTokenResult.AccessToken,
                    TokenType = accessTokenResult.TokenType,
                    ExpiresIn = accessTokenResult.ExpiresIn,
                    DateIssued = accessTokenResult.Issued,
                    DateExpires = accessTokenResult.Expires
                };

                var addAccessTokenResult = await _accessTokenClient.AddAccessTokenAsync(newAccessToken, cancellationToken);

                if (addAccessTokenResult <= 0)
                {
                    _logger.LogWarning((int)LoggingEvents.InsertItem, "PushNotificationManager.SendAsync - _accessTokenClient.AddAccessTokenAsync returned no results");
                }

                accessToken = accessTokenResult.AccessToken;
                tokenType = accessTokenResult.TokenType;
            }

            var pushNotificationRequest = new PushNotificationRequest
            {
                IdNumber = idNumber,
                Title = title,
                Message = message
            };

            var pushNotificationResult = await _pushNotificationClient.SendPushNotification(accessToken, tokenType, pushNotificationRequest, cancellationToken);

            if (pushNotificationResult == null)
            {
                _logger.LogError((int)LoggingEvents.ExternalCall, "PushNotificationManager.SendAsync - _pushNotificationClient.SendPushNotification returned null", pushNotificationRequest);
                errorResponse.StatusCode = HttpStatusCode.ServiceUnavailable;
                errorResponse.AdditionalErrorMessage = "Error creating a Push Notification request.";
                return errorResponse;
            }

            if (pushNotificationResult.Success)
            {
                return new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = pushNotificationResult.Message
                };
            }

            errorResponse.AdditionalErrorMessage = pushNotificationResult.Message;
            return errorResponse;
        }
        
    }
}
