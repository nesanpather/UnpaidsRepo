﻿using System;
using System.Collections.Generic;

namespace DataManager.Models
{
    public partial class TbStatus
    {
        public TbStatus()
        {
            TbUnpaidBatch = new HashSet<TbUnpaidBatch>();
            TbUnpaidRequest = new HashSet<TbUnpaidRequest>();
            TbUnpaidResponse = new HashSet<TbUnpaidResponse>();
        }

        public int StatusId { get; set; }
        public string Status { get; set; }

        public virtual ICollection<TbUnpaidBatch> TbUnpaidBatch { get; set; }
        public virtual ICollection<TbUnpaidRequest> TbUnpaidRequest { get; set; }
        public virtual ICollection<TbUnpaidResponse> TbUnpaidResponse { get; set; }
    }
}
