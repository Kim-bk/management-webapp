using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests
{
    public class ListTaskRequest
    {
        [Required]
        public int ProjectId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
    }
}
