using System.Collections.Generic;
using Domain.Entities;

namespace Domain.DTOs.Responses
{
    public class ProjectManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<ProjectDTO> Projects { get; set; }
        public List<ListTaskDTO> ListTaskDTOs { get; set; }
        public ProjectDTO Project { get; set; }
    }
}
