using System;
using System.Collections.Generic;

#nullable disable

namespace API.Entities
{
    public partial class Project
    {
        public Project()
        {
            ListTasks = new HashSet<ListTask>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ListTask> ListTasks { get; set; }
    }
}
