using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public partial class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
