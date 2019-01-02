﻿using System;

namespace UnpaidModels
{
    public class UnpaidResponse
    {
        public int UnpaidResponseId { get; set; }
        public int UnpaidRequestId { get; set; }
        public int ResponseId { get; set; }
        public bool Accepted { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
