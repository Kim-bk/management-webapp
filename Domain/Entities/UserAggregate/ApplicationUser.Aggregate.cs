using Domain.Base;

namespace Domain.AggregateModels.UserAggregate
{
    public partial class ApplicationUser : IAggregateRoot
    {
        public void CreateRefreshToken(string tokenId, string token)
        {
            var refreshToken = new RefreshToken
            {
                Id = tokenId,
                Token = token,
            };
            RefreshToken = refreshToken;
        }
    }
}
