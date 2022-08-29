using System.Collections.Generic;
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.TaskAggregate;
using Domain.Base;
using Domain.SeedWork;
using Microsoft.AspNetCore.Identity;


namespace Domain.AggregateModels.UserAggregate
{
    public class ApplicationUser : IdentityUser, IAggregateRoot
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
