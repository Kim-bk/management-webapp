using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectRepository: Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbFactory dbFactory) : base(dbFactory)
        { }

        public async void CreateProject(Project project)
        {
            await DbSet.AddAsync(project);
        }

        public async Task<Project> FindByIdAsync(int projectId)
        {
            return await DbSet.Where(x => x.Id == projectId).FirstOrDefaultAsync();
        }

        public void UpdateProject(Project project)
        {
            DbSet.Update(project);
        }
    }
}
