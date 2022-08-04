using System.ComponentModel.DataAnnotations;

namespace Domain.DTOS.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set;}
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }
}
