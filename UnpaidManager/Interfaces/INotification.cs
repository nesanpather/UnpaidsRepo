using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface INotification
    {
        Task<NotificationResponse> SendAsync(string title, string message, string idNumber, string correlationId, CancellationToken cancellationToken);
    }
}
