﻿namespace UnpaidModels
{
    public class UnpaidResponseInput
    {
        public string PolicyNumber { get; set; }
        public string IdNumber { get; set; }
        public bool Accepted { get; set; }
        public ContactOption ContactOption { get; set; }
    }
}
