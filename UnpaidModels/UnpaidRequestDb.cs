using System;
using System.ComponentModel.DataAnnotations;

namespace UnpaidModels
{
    public class UnpaidRequestDb
    {
        [Key]
        public int UnpaidRequestId { get; set; }
        public int UnpaidId { get; set; }
        public int NotificationId { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public string StatusAdditionalInfo { get; set; }
    }
}
