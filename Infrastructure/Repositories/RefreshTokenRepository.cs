using System;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Domain.AggregateModels.UserAggregate;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DbFactory dbFactory) : base(dbFactory)
        { }
 
        public async Task DeleteAll(string userId)
        {
            var listTokens = await DbSet.Where(u => u.UserId == userId).ToListAsync();
            DbSet.RemoveRange(listTokens);
        }
    }
}
