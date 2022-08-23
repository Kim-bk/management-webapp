using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    [Table("User")]
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Tasks = new HashSet<Task>();
            Projects = new HashSet<Project>();
        }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
