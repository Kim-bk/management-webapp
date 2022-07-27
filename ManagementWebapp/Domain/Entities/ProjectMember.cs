using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

#nullable disable

namespace API.Entities
{
    public partial class ProjectMember
    {
        [Key]
        public int? ProjectId { get; set; }
        public string UserId { get; set; }

        public virtual Project Project { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}
