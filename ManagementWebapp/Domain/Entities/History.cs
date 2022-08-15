using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Domain.Entities
{
    public partial class History
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; }
        public string AffectedColumn { get; set; }
        public int PrimaryKey { get; set; }
    }
}
