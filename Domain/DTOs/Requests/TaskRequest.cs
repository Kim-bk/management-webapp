using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests
{ 
    public class TaskRequest
    {
        [Required]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        [Required]
        public int ListTaskId { get; set; }
    }
}
