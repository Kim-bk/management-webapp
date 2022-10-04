using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests
{
    public class MoveTaskRequest
    {
        [Required]
        public int TaskId { get; set; }
    }
}
