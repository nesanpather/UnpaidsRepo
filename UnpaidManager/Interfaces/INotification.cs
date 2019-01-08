using System.Threading;
using System.Threading.Tasks;

namespace UnpaidManager.Interfaces
{
    public interface INotification
    {
        Task<bool> SendAsync(string title, string message, string idNumber, CancellationToken cancellationToken);
    }
}
