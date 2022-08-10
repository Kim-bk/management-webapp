using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        System.Threading.Tasks.Task Create(RefreshToken refreshToken);
        System.Threading.Tasks.Task DeleteAll(string userId);
        Task<RefreshToken> FindByIdAsync(string refreshTokenId);
        System.Threading.Tasks.Task Delete(string tokenId);
        Task<RefreshToken> FindByToken(string token);
    }
}
