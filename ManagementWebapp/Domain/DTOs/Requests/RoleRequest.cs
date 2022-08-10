using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Requests
{
    public class RoleRequest
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài {2} đến {1} ký tự!")]
        public string RoleName { get; set; }
    }
}
