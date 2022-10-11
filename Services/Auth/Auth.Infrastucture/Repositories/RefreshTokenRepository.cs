using System;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Domain.AggregateModels.UserAggregate;
using System.Data.Entity;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DbFactory dbFactory) : base(dbFactory)
        { }
 
        public async Task DeleteAll(string userId)
        {
            var listTokens = await GetQuery(u => u.UserId == userId).ToListAsync();
            Delete(listTokens);
        }
    }
}
