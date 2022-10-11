using System.Collections.Generic;

namespace API.DTOs.Responses
{
    public class ProjectManagerResponse
    {
        public string Message { get; set; }
        public List<ListTaskDTO> ListTasks { get; set; }
        public List<ProjectDTO> Projects { get; set; }
    }
}
