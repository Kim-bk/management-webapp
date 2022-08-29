using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.CustomIdentity
{
    public class CustomRoleStore : RoleStore<IdentityRole>
    {
        public CustomRoleStore(AppDbContext context) : base(context)
        {
            AutoSaveChanges = false;
        }
    }
}
