using Domain.Base;

namespace Domain.AggregateModels.UserAggregate
{
    public partial class ApplicationUser : IAggregateRoot
    {
        public RefreshToken CreateRefreshToken(string tokenId, string token)
        {
            return new RefreshToken
            {
                Id = tokenId,
                Token = token,
            };
        }
    }
}
