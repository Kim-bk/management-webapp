using System.Collections.Generic;

namespace API.DTOs
{
    public class TaskDTO
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public int? Position { get; set; }
        public List<TodoDTO> Todos { get; set; }
        public List<MemberDTO> Members { get; set; }
        public List<LabelDTO> Labels { get; set; }
    }
}
