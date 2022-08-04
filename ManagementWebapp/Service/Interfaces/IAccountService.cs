using System.Threading.Tasks;
using Domain.DTOs.Responses;
using Domain.DTOS.Requests;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model);
        Task<UserManagerResponse> UserLoginAsync(LoginRequest model);
        ProjectManagerResponse GetUserProjects(string userId);
        Task<UserManagerResponse> UserLogout();
    }
}
