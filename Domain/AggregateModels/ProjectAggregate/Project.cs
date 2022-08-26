using System.Collections.Generic;
using Domain.AggregateModels.UserAggregate;

#nullable disable

namespace Domain.AggregateModels.ProjectAggregate
{
    public partial class Project
    {
        public Project()
        {
            ListTasks = new HashSet<ListTask>();
            Users = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ListTask> ListTasks { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    
    }
}
