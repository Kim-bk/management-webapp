﻿using System.ComponentModel.DataAnnotations;

namespace Domain.DTOS.Requests
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
    }
}
