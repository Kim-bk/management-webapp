using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.AggregateModels.ProjectAggregate;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectRepository: Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public void CreateProject(Project project)
        {
            AddAsync(project);
        }

        public async Task<Project> FindByIdAsync(int projectId)
        {
            return await DbSet.Where(x => x.Id == projectId).FirstOrDefaultAsync();
        }

        public async Task<Project> FindByNameAsync(string name)
        {
            return await DbSet.Where(p => p.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<ListTask>> GetListTasksByProjectId(int projectId)
        {
            return await DbSet.Where(p => p.Id == projectId)
                        .SelectMany(lt => lt.ListTasks)
                        .ToListAsync();
        }

    }
}
