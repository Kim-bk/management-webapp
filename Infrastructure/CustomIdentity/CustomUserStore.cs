using API.Context;
using Domain.AggregateModels.UserAggregate;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Custom
{
    public class CustomUserStore : UserStore<ApplicationUser>
    {
        public CustomUserStore(AppDbContext context) : base(context)
        {
            AutoSaveChanges = false;
        }
    }
}
