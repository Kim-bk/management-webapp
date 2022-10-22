using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.AggregateModels.UserAggregate;

#nullable disable
namespace Domain.AggregateModels.UserAggregate
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
