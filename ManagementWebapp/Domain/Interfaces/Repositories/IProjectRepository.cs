﻿using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        void CreateProject(Project project);
        Task<Project> FindByIdAsync(int projectId);
        Task<ProjectDTO> GetListTasksByProjectId(int projectId);
        Task<Project> FindByNameAsync(string name);
    }
}
