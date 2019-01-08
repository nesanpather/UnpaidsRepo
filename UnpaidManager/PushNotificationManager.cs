﻿using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager
{
    public class PushNotificationManager : INotification
    {
        private readonly IPushNotificationClient _pushNotificationClient;        
        // need to inject an AccessTokenManager to manage how long to use the same access token for bulk processing;

        public PushNotificationManager(IPushNotificationClient pushNotificationClient)
        {
            _pushNotificationClient = pushNotificationClient;
        }

        public async Task<NotificationResponse> SendAsync(string title, string message, string idNumber, CancellationToken cancellationToken)
        {
            var errorResponse = new NotificationResponse
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            if (string.IsNullOrWhiteSpace(title))
            {
                // Log Error.
                errorResponse.AdditionalErrorMessage = "Title is null or empty.";
                return errorResponse;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                // Log Error.
                errorResponse.AdditionalErrorMessage = "Message is null or empty.";
                return errorResponse;
            }

            if (string.IsNullOrWhiteSpace(idNumber))
            {
                // Log Error.
                errorResponse.AdditionalErrorMessage = "IdNumber is null or empty.";
                return errorResponse;
            }

            var accessTokenResult = await _pushNotificationClient.GetAccessTokenAsync(cancellationToken);

            if (accessTokenResult == null)
            {
                // Log Error.
                errorResponse.StatusCode = HttpStatusCode.ServiceUnavailable;
                errorResponse.AdditionalErrorMessage = "Error getting a WebToken.";
                return errorResponse;
            }

            var pushNotificationRequest = new PushNotificationRequest
            {
                IdNumber = idNumber,
                Title = title,
                Message = message
            };

            var pushNotificationResult = await _pushNotificationClient.SendPushNotification(accessTokenResult.AccessToken, accessTokenResult.TokenType, pushNotificationRequest, cancellationToken);

            if (pushNotificationResult == null)
            {
                // Log Error.               
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
