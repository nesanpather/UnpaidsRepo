using System;
using System.Collections.Generic;

namespace DataManager.Models
{
    public partial class TbResponse
    {
        public TbResponse()
        {
            TbUnpaidResponse = new HashSet<TbUnpaidResponse>();
        }

        public int ResponseId { get; set; }
        public string Response { get; set; }

        public virtual ICollection<TbUnpaidResponse> TbUnpaidResponse { get; set; }
    }
}
