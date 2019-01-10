using System;
using System.Collections.Generic;

namespace DataManager.Models
{
    public partial class TbNotification
    {
        public TbNotification()
        {
            TbUnpaidRequest = new HashSet<TbUnpaidRequest>();
        }

        public int NotificationId { get; set; }
        public string Notification { get; set; }

        public virtual ICollection<TbUnpaidRequest> TbUnpaidRequest { get; set; }
    }
}
