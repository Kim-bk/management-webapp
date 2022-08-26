using System;
using System.Collections.Generic;
using Domain.AggregateModels.ProjectAggregate;

namespace API.DTOs.Responses
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors {get;set;}
        public DateTime? ExpireDate { get; set; }
        public List<Project> Project { get; set; }
    }
}
