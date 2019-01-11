using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using UnpaidModels;

namespace UnpaidManager.Interfaces
{
    public interface IUnpaidResponseClient
    {
        Task<int> AddPendingUnpaidResponseAsync(UnpaidResponseInput unpaidResponseInput, int unpaidRequestId, CancellationToken cancellationToken);
        Task<IEnumerable<TbUnpaidResponse>> GetUnpaidResponseAsync(int unpaidRequestId, CancellationToken cancellationToken);
        Task<IEnumerable<GetAllUnpaidResponseOutput>> GetAllUnpaidResponseAsync(CancellationToken cancellationToken);
        Task<IEnumerable<GetAllUnpaidResponseOutput>> GetAllUnpaidResponseAsync(string policyNumber, CancellationToken cancellationToken);
    }
}
