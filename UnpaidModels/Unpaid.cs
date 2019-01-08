using System;

namespace UnpaidModels
{
    public class Unpaid
    {
        public int UnpaidId { get; set; }
        public string PolicyNumber { get; set; }
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
