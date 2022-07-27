using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

#nullable disable

namespace Domain.Entities
{
    public partial class Task
    {
        public Task()
        {
            Todos = new HashSet<Todo>();
        }

        public int Id { get; set; }
        public int? ListTaskId { get; set; }
        public string Title { get; set; }
        public int? Position { get; set; }
        public string UserId { get; set; }

        public virtual ListTask ListTask { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Todo> Todos { get; set; }
    }
}
