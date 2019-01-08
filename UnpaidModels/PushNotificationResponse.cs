using Newtonsoft.Json;

namespace UnpaidModels
{
    public class PushNotificationResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }       
    }
}
