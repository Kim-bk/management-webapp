using System;
using System.Collections.Generic;

#nullable disable

namespace API.Entities
{
    public partial class LabelTask
    {
        public int? LabelId { get; set; }
        public int? TaskId { get; set; }

        public virtual Label Label { get; set; }
        public virtual Task Task { get; set; }
    }
}
