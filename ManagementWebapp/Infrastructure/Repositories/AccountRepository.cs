using System.Collections.Generic;
using System.Linq;
using Domain.Accounts;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        public AccountRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
        public IQueryable<ICollection<Project>> GetUserProjects(string userId)
        {
            return DbSet.Where(u => u.Id == userId).Select(p => p.Projects);
        }
    }
}
