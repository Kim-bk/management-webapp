using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Service.DTOs.Requests
{
    public class CommonRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
    }
}

