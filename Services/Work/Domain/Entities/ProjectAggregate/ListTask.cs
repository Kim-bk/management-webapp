using System.Collections.Generic;
using Domain.AggregateModels.TaskAggregate;
using Domain.SeedWork;

namespace Domain.AggregateModels.ProjectAggregate
{
    public partial class ListTask
    {
        public ListTask()
        {
            Tasks = new HashSet<Task>();
        }

        public int ListTaskId { get; set; }
        public int? ProjectId { get; set; }
        public string Title { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
