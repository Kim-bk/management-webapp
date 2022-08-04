using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public List<ListTaskDTO> ListTasks { get; set; }
    }
}
