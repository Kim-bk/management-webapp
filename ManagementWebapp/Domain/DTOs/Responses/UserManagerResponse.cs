using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.DTOs.Responses
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors {get;set;}
        public DateTime? ExpireDate { get; set; }
        public List<Project> Project { get; set; }
    }
}
