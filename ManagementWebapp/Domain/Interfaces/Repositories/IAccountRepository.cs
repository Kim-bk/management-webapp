using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Domain.Accounts
{
    public interface IAccountRepository
    {
        Task<IdentityResult> RegisterUser(RegisterRequest model);
        Task<IdentityUser> FindUserByName(string name);
        Task<bool> CheckPassword(IdentityUser user, string password);
        IList<Project> GetUserProjects(string userId);
        Project CreateUserProject(ProjectRequest model);
        bool AssignUserToProject(Project project, string userId);
        Task<bool> UserLogout(); 
    }
}
