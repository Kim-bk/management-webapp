using Domain.AggregateModels.UserAggregate;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task DeleteAll(string userId);
    }
}
