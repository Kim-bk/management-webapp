using System.Collections.Generic;
using Domain.Entities;

namespace API.DTOs.Responses
{
    public class ListTaskManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<TaskDTO> Task { get; set; }
        public int ProjectId { get; set; }
    }
}
