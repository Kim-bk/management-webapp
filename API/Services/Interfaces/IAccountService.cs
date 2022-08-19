using System.Threading.Tasks;
using API.DTOs.Responses;
using API.DTOs.Requests;
using Domain.Entities;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model);
        Task<ApplicationUser> UserLoginAsync(LoginRequest model);
        Task<ProjectManagerResponse> GetUserProjects(string userId);
        Task<UserManagerResponse> Logout(string userId);
    }
}
