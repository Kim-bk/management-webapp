using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Entities
{
    public partial class Todo
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public string Title { get; set; }
        public bool? IsDone { get; set; }
        public int? ParentId { get; set; }
        public virtual Task Task { get; set; }
    }
}
