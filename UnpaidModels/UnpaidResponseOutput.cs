using System.Net;

namespace UnpaidModels
{
    public class UnpaidResponseOutput: UnpaidResponseInput
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
