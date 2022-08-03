using System.Collections.Generic;
using Domain.Entities;

namespace Service.DTOs.Responses
{
    public class TaskManagerResponse
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public bool IsSuccess { get; set; }
        public string MemberDoing { get; set; }
        public List<Label> Labels { get; set; }
        public List<Todo> Todos { get; set; }
        public List<ApplicationUser> Members { get; set; }
    }
}
