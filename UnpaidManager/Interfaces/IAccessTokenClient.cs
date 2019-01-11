using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;

namespace UnpaidManager.Interfaces
{
    public interface IAccessTokenClient
    {
        Task<int> AddAccessTokenAsync(TbAccessToken accessToken, CancellationToken cancellationToken);
        Task<TbAccessToken> GetLatestAccessTokenAsync(CancellationToken cancellationToken);
    }
}
