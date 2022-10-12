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

        public void Update(string email, string phoneNumber)
        {
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
