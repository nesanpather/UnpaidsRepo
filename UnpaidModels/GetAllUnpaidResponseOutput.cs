using System;

namespace UnpaidModels
{
    public class GetAllUnpaidResponseOutput
    {
        public int UnpaidId { get; set; }
        public string PolicyNumber { get; set; }
        public string IdNumber { get; set; }
        public DateTime DateAdded { get; set; }
        public int NotificationRequestId { get; set; }
        public string NotificationType { get; set; }
        public DateTime DateNotificationSent { get; set; }
        public string ContactOptionType { get; set; }
        public string ContactOptionStatus { get; set; }
        public bool Accepted { get; set; }
        public DateTime DateNotificationResponseAdded { get; set; }
        public string CorrelationId { get; set; }
    }
}
