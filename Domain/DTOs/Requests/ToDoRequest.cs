using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests
{
    public class ToDoRequest
    {
        [Required]
        public int TaskId { get; set; }
        [Required]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public int? ParentId { get; set; }
    }
}
