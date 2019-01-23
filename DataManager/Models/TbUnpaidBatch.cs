using System;
using System.Collections.Generic;

namespace DataManager.Models
{
    public partial class TbUnpaidBatch
    {
        public TbUnpaidBatch()
        {
            TbUnpaid = new HashSet<TbUnpaid>();
        }

        public int UnpaidBatchId { get; set; }
        public string BatchKey { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual TbStatus Status { get; set; }
        public virtual ICollection<TbUnpaid> TbUnpaid { get; set; }
    }
}
