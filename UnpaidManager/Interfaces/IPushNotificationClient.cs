using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IPushNotificationClient
    {
        Task<PushNotificationWebTokenResponse> GetAccessTokenAsync(CancellationToken cancellationToken);

        Task<PushNotificationResponse> SendPushNotification(string accessToken, string tokenType, PushNotificationRequest pushNotificationRequest, CancellationToken cancellationToken);
    }
}
