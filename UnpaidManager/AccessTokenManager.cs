using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using UnpaidManager.Interfaces;

namespace UnpaidManager
{
    public class AccessTokenManager: IAccessTokenClient
    {
        private readonly IAccessTokenStorageOperations _accessTokenStorageOperations;

        public AccessTokenManager(IAccessTokenStorageOperations accessTokenStorageOperations)
        {
            _accessTokenStorageOperations = accessTokenStorageOperations;
        }

        public async Task<int> AddAccessTokenAsync(TbAccessToken accessToken, CancellationToken cancellationToken)
        {
            return await _accessTokenStorageOperations.AddUnpaidRequestAsync(accessToken, cancellationToken);
        }

        public async Task<TbAccessToken> GetLatestAccessTokenAsync(CancellationToken cancellationToken)
        {
            return await _accessTokenStorageOperations.GetLatestAccessTokenAsync(cancellationToken);
        }
    }
}
