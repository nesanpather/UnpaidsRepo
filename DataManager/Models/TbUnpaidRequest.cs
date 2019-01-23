using System;
using System.Collections.Generic;

namespace DataManager.Models
{
    public partial class TbUnpaidRequest
    {
        public TbUnpaidRequest()
        {
            TbUnpaidResponse = new HashSet<TbUnpaidResponse>();
        }

        public int UnpaidRequestId { get; set; }
        public int UnpaidId { get; set; }
        public int NotificationId { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public string StatusAdditionalInfo { get; set; }
        public DateTime? DateModified { get; set; }
        public string CorrelationId { get; set; }

        public virtual TbNotification Notification { get; set; }
        public virtual TbStatus Status { get; set; }
        public virtual TbUnpaid Unpaid { get; set; }
        public virtual ICollection<TbUnpaidResponse> TbUnpaidResponse { get; set; }
    }
}
