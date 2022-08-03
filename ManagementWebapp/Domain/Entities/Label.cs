using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Entities
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
