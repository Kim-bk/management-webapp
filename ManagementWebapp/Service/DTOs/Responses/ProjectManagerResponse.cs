using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Service.DTOs.Responses
{
    public class ProjectManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<Project> Project { get; set; }
    }
}
