using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.AggregateModels.UserAggregate
{
    public partial class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }
        public virtual RefreshToken RefreshToken { get; set; }
    }
}
