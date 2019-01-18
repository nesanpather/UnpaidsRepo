using System;
using System.Collections.Generic;

namespace DataManager.Models
{
    public partial class TbUnpaidBatch
    {
        public int UnpaidBatchId { get; set; }
        public string IdempotencyKey { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual TbStatus Status { get; set; }
    }
}
