using System;

namespace UnpaidModels
{
    public class GetAllUnpaidRequestOutput
    {
        public int UnpaidId { get; set; }
        public string PolicyNumber { get; set; }
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime DateAdded { get; set; }
        public int NotificationRequestId { get; set; }
        public string NotificationType { get; set; }
        public string NotificationSentStatus { get; set; }
        public string NotificationErrorMessage { get; set; }
        public DateTime DateNotificationSent { get; set; }
    }
}
