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
        public async Task<List<Project>> GetAll()
        {
            return await DbSet.ToListAsync();
        }
    }
}
