using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
