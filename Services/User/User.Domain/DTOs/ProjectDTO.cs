using System.Collections.Generic;

namespace API.DTOs
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public List<ListTaskDTO> ListTasks { get; set; }
    }
}
