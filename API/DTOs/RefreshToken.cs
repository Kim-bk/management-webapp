using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTOs
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
