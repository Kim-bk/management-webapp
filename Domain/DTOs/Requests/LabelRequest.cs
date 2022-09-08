using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests
{
    public class LabelRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int TaskId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Color { get; set; }
    }
}
