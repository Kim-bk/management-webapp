using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        void CreateProject(Project project);
        Task<Project> FindByIdAsync(int projectId);
        void UpdateProject(Project project);
    }
}
