using System.Collections.Generic;
using Domain.Entities;

#nullable disable

namespace Domain.Entities
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
