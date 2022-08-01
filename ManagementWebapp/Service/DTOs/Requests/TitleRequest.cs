using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Service.DTOs.Requests
{
    public class TitleRequest
    {
        [Required]
        public string Title { get; set; }
    }
}

