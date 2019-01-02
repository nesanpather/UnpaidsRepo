using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataManager.Interfaces;
using UnpaidModels;

namespace DataManager
{
    public class UnpaidResponseDataManager: IUnpaidResponseOperations
    {
        public Task<int> AddUnpaidResponseAsync(UnpaidResponse unpaidResponse)
        {
            throw new NotImplementedException();
        }

        public Task<UnpaidResponse> GetUnpaidResponseAsync(int unpaidRequestId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UnpaidResponse>> GetAllUnpaidResponseAsync()
        {
            throw new NotImplementedException();
        }
    }
}
