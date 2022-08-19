using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DbFactory dbFactory) : base(dbFactory)
        { }
        public System.Threading.Tasks.Task Create(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid().ToString();
            DbSet.AddAsync(refreshToken);
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public void DeleteAll(RefreshToken refreshToken)
        {
            DbSet.Remove(refreshToken);
        }
        public async System.Threading.Tasks.Task Delete(string tokenId)
        {
            var refreshToken = await this.FindByIdAsync(tokenId);
            if (refreshToken != null)
                DbSet.Remove(refreshToken);
        }
        public async Task<RefreshToken> FindByIdAsync(string refreshTokenId)
        {
            return await DbSet.FindAsync(refreshTokenId);
        }
        public async Task<RefreshToken> FindByToken(string token)
        {
            return await DbSet.Where(t => t.Token == token).FirstOrDefaultAsync();
        }

        public async System.Threading.Tasks.Task DeleteAll(string userId)
        {
            var listTokens = await DbSet.Where(u => u.UserId == userId).ToListAsync();
            DbSet.RemoveRange(listTokens);
        }
    }
}
