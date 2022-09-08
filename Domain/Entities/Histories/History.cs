using System;

#nullable disable
namespace Domain.Entities.Histories
{
    public partial class History
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumn { get; set; }
        public string PrimaryKey { get; set; }
    }
}
