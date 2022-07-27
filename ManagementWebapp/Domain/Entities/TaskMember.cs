using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class TaskMember
    {
        [Key]
        public int? TaskId { get; set; }
        public string UserId { get; set; }
        public virtual Task Task { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
