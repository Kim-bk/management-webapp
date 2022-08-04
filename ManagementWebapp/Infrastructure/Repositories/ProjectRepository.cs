using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectRepository: Repository<Project>, IProjectRepository
    {
        private IMapper _mapper;
        public ProjectRepository(DbFactory dbFactory, IMapper mapper) : base(dbFactory)
        {
            _mapper = mapper;
        }

        public async void CreateProject(Project project)
        {
            await DbSet.AddAsync(project);
        }

        public async Task<Project> FindByIdAsync(int projectId)
        {
            return await DbSet.Where(x => x.Id == projectId).FirstOrDefaultAsync();
        }

        public async Task<Project> FindByNameAsync(string name)
        {
            return await DbSet.FindAsync(name);
        }

        public async Task<ProjectDTO> GetListTasksByProjectId(int projectId)
        {
            var project = await DbSet.FindAsync(projectId);
            return new ProjectDTO 
            { 
                ListTasks = _mapper.MapListTasks(project),
            };
        }
    }
}
