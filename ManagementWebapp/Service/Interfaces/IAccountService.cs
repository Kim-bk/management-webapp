using System.Threading.Tasks;
using Domain.DTOs.Responses;
using Domain.DTOS.Requests;
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
