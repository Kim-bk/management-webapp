using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Accounts
{
    public interface IAccountRepository
    {
        IList<Project> GetUserProjects(string userId);
        Project CreateUserProject(string projectName);
        bool AddUserToProject(Project project, string userId);
    }
}
