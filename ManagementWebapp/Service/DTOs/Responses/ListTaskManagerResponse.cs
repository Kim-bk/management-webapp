using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Service.DTOs.Responses
{
    public class ListTaskManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<Task> Task { get; set; }
    }
}
