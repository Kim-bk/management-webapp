using System.Collections.Generic;
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;
using Domain.SeedWork;

namespace Domain.AggregateModels.TaskAggregate
{
    public partial class Task
    {
        public Task()
        {
            Todos = new HashSet<Todo>();
            Labels = new HashSet<Label>();
            Users = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public int? ListTaskId { get; set; }
        public string Title { get; set; }
        public int? Position { get; set; }
        public string? DoingId { get; set; }
        public virtual ListTask ListTask { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Todo> Todos { get; set; }
        public virtual ICollection<Label> Labels { get; set; }
    }
}
