using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.AggregateModels.ProjectAggregate;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> CreateProject(Project project);
        Task<Project> FindByIdAsync(int projectId);
        Task<List<ListTask>> GetListTasksByProjectId(int projectId);
        Task<Project> FindByNameAsync(string name);
        Task<List<Project>> GetAll();
    }
}
