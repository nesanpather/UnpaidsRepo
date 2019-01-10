using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;

namespace DataManager.Interfaces
{
    public interface IAccessTokenStorageOperations
    {
        Task<int> AddUnpaidRequestAsync(TbAccessToken accessToken, CancellationToken cancellationToken);
        Task<TbAccessToken> GetLatestAccessTokenAsync(CancellationToken cancellationToken);
    }
}
