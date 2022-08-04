using System.Collections.Generic;
using Domain.Entities;

namespace Domain.DTOs.Responses
{
    public class ListTaskManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<TaskDTO> Task { get; set; }
    }
}
