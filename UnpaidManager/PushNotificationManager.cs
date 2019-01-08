using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnpaidManager.Interfaces;
using UnpaidModels;
using Utilities.Interfaces;

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

        public Task<bool> SendAsync(string title, string message, string idNumber, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                // Log Error.
                return null;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                // Log Error.
                return null;
            }

            if (string.IsNullOrWhiteSpace(idNumber))
            {
                // Log Error.
                return null;
            }


            throw new NotImplementedException();
        }
        
    }
}
