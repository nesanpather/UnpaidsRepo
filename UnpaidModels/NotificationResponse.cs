using System.Net;

namespace UnpaidModels
{
    public class NotificationResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string AdditionalErrorMessage { get; set; }
    }
}
