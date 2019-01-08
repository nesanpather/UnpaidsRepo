using System.Threading;
using System.Threading.Tasks;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    interface IEnterpriseServerOperations
    {
        Task<ClientDetails> GetClientDetailsAsync(string referenceNumber, CancellationToken cancellationToken);
    }
}
