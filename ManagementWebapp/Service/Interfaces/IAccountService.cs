using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Service.DTOs.Responses;
using Service.DTOS.Requests;
using Service.DTOS.Responses;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        // User to register
        Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model);

        // User to login
        Task<UserManagerResponse> UserLoginAsync(LoginRequest model);

        // Get all projects of User login
        Task<ProjectManagerResponse> GetUserProjects(string userId);

        // User to logout
        Task<UserManagerResponse> UserLogout();
    }
}
