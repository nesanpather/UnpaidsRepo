using System;
using System.Threading;
using System.Threading.Tasks;
using UnpaidManager.Interfaces;
using UnpaidModels;
using Utilities.Interfaces;

namespace UnpaidManager
{
    public class TapeClientManager : IEnterpriseServerOperations
    {
        private readonly IHttpClientOperations _httpClientOperations;

        public TapeClientManager(IHttpClientOperations httpClientOperations)
        {
            _httpClientOperations = httpClientOperations;
        }

        public Task<ClientDetails> GetClientDetailsAsync(string referenceNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
