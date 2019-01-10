using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Interfaces;
using DataManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DataManager
{
    public class AccessTokenDataManager: IAccessTokenStorageOperations
    {
        private readonly UnpaidsContext _unpaidsDbContext;

        public AccessTokenDataManager(UnpaidsContext unpaidsDbContext)
        {
            _unpaidsDbContext = unpaidsDbContext;
        }

        public async Task<int> AddUnpaidRequestAsync(TbAccessToken accessToken, CancellationToken cancellationToken)
        {
            _unpaidsDbContext.TbAccessToken.Add(accessToken);
            return await _unpaidsDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TbAccessToken> GetLatestAccessTokenAsync(CancellationToken cancellationToken)
        {
            return await _unpaidsDbContext.TbAccessToken.OrderByDescending(a => a.DateExpires).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
