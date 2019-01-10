using System;
using System.Collections.Generic;

namespace DataManager.Models
{
    public partial class TbUnpaidResponse
    {
        public int UnpaidResponseId { get; set; }
        public int UnpaidRequestId { get; set; }
        public int ResponseId { get; set; }
        public bool Accepted { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual TbResponse Response { get; set; }
        public virtual TbStatus Status { get; set; }
        public virtual TbUnpaidRequest UnpaidRequest { get; set; }
    }
}
