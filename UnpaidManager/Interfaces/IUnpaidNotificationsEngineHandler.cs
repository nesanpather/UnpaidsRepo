using System.Threading;
using System.Threading.Tasks;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidNotificationsEngineHandler
    {
        Task HandleUnpaidNotificationsAsync(string idempotencyKey, CancellationToken cancellationToken);
    }
}
