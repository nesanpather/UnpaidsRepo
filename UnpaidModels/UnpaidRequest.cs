using System;

namespace UnpaidModels
{
    public class UnpaidRequest
    {
        public int UnpaidRequestId { get; set; }
        public int UnpaidId { get; set; }
        public int NotificationId { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
