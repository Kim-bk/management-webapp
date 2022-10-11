using System.Collections.Generic;

namespace API.DTOs
{
    public class ListTaskDTO
    {
        public int LiskTaskId { get; set; }
        public string Title { get; set; }
        public List<TaskDTO> Tasks { get; set; } 
    }
}
