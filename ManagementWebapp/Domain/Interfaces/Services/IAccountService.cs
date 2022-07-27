using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using Domain.Shared;

namespace Domain.Intefaces.Services
{
    public interface IAccountService
    {
        // User to register
        Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model);

        // User to login
        Task<UserManagerResponse> UserLoginAsync(LoginRequest model);

        // Get all projects of User login
        Task<IList<Project>> GetUserProjects(string userId);

        // User to logout
        Task<UserManagerResponse> UserLogout();

        // User to create new project
        Task<UserManagerResponse> CreateUserProject(ProjectRequest model, string userId);
    }

}
