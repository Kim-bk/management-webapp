using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Entities
{
    public partial class History
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedById { get; set; }
        public string AtTable { get; set; }
        public int? ReferenceId { get; set; }
    }
}
