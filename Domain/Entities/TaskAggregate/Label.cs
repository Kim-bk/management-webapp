using System.Collections.Generic;
using Domain.SeedWork;

#nullable disable

namespace Domain.AggregateModels.TaskAggregate
{
    public partial class Label
    {
        public Label()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
