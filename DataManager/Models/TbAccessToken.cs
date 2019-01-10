using System;
using System.Collections.Generic;

namespace DataManager.Models
{
    public partial class TbAccessToken
    {
        public int AccessTokenId { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime DateExpires { get; set; }
    }
}
