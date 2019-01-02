using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataManager.Interfaces;
using UnpaidModels;

namespace DataManager
{
    public class UnpaidRequestDataManager: IUnpaidRequestOperations
    {
        public Task<int> AddUnpaidRequestAsync(UnpaidRequest unpaidRequest)
        {
            throw new NotImplementedException();
        }

        public Task<UnpaidRequest> GetUnpaidRequestAsync(int unpaidId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UnpaidRequest>> GetAllUnpaidRequestAsync()
        {
            throw new NotImplementedException();
        }
    }
}
