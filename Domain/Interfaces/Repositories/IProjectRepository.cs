using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        void CreateProject(Project project);
        Task<Project> FindByIdAsync(int projectId);
        Task<List<ListTask>> GetListTasksByProjectId(int projectId);
        Task<Project> FindByNameAsync(string name);
    }
}
