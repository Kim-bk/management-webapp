using System.Collections.Generic;

namespace Domain.DTOs
{   
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ListTaskDTO> ListTasks {get; set; }
    }
}
