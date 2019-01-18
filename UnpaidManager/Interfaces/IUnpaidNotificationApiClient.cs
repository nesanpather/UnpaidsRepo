using System.Threading;
using System.Threading.Tasks;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidNotificationApiClient
    {
        Task<string> DelegateToNotificationApiAsync(string idempotencyKey, CancellationToken cancellationToken);
    }
}
