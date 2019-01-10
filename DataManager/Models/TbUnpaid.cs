using System;
using System.Collections.Generic;

namespace DataManager.Models
{
    public partial class TbUnpaid
    {
        public TbUnpaid()
        {
            TbUnpaidRequest = new HashSet<TbUnpaidRequest>();
        }

        public int UnpaidId { get; set; }
        public string PolicyNumber { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public string IdNumber { get; set; }
        public string IdempotencyKey { get; set; }

        public virtual ICollection<TbUnpaidRequest> TbUnpaidRequest { get; set; }
    }
}
