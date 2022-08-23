using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace Domain.Entities
{
    [Table("RefreshToken")]
    public partial class RefreshToken
    {
        [Key]
        public string Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }
}
