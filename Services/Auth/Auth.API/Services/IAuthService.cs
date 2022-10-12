using API.DTOs.Responses;
using Auth.Domain.DTOs.Requests;
using Domain.AggregateModels.UserAggregate;
using System.Threading.Tasks;

namespace Auth.API.Services
{
    public interface IAuthService
    {
        public Task<AuthenticatedUserResponse> Authenticate(ApplicationUser user);
    }
}
