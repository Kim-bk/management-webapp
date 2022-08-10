using System;
using System.Threading.Tasks;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;

namespace Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IListTaskRepository _listTaskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProjectService(IProjectRepository projectRepository, IListTaskRepository listTaskRepository,
                            IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _projectRepository = projectRepository;
            _listTaskRepository = listTaskRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        
        public async Task<UserManagerResponse> AddMemberToProject(int projectId, ProjectRequest model)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find user in Database
                var user = await _userManager.FindByIdAsync(model.UserId);

                // 2. Find project in Database
                var project = await _projectRepository.FindByIdAsync(projectId);

                // 3. Add member to project then update
                project.Users.Add(user);

                await _unitOfWork.CommitTransaction();

                // 4. Return message
                return new UserManagerResponse
                {
                    Message = "Add member to project success!",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                return new UserManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = false
                };
            }
        }
        public async Task<UserManagerResponse> CreateListTask(int projectId, CommonRequest model)
        {
            try
            {
                // 1.Find project by it ID
                var project = await _projectRepository.FindByIdAsync(projectId);
                if (project == null)
                {
                    return new UserManagerResponse
                    {
                        Message = "The Project is not found!",
                        IsSuccess = true,
                    };
                }

                // 2. Check if list task is duplicated
                var listTask = await _listTaskRepository.FindByNameAsync(model.Title);
                if (listTask != null && listTask.ProjectId == projectId)
                {
                    return new UserManagerResponse
                    {
                        Message = "The List Task already exists in the project",
                        IsSuccess = true,
                    };
                }

                await _unitOfWork.BeginTransaction();

                // 3. Create list task in the project then update
                project.ListTasks.Add(new ListTask
                {
                    Title = model.Title,
                    Project = project
                });

                await _unitOfWork.CommitTransaction();

                // 4. Return message
                return new UserManagerResponse
                {
                    Message = "Create list task success",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                return new UserManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = true
                };
            }
        }

        public async Task<UserManagerResponse> CreateUserProject(string userId, ProjectRequest model)
        {
            try
            {
                // 1. Find project by name
                var project = await _projectRepository.FindByNameAsync(model.Name);

                // 2. Find user
                var user = await _userManager.FindByIdAsync(userId);

                // 3. Check if name project is exist in user projects
                if (project != null && project.Users.Contains(user))
                {
                    return new UserManagerResponse
                    {
                        Message = "The project already exists",
                        IsSuccess = true,
                    };
                }

                await _unitOfWork.BeginTransaction();

                // 4. Create Project then add to user
                user.Projects.Add(new Project
                {
                    Name = model.Name,
                });

                // 5. Commit transaction if not catch exception
                await _unitOfWork.CommitTransaction();

                // 6. Return message create success
                return new UserManagerResponse
                {
                    Message = "Create new project to user!",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                return new UserManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = true
                };
            }
        }

        public async Task<ProjectManagerResponse> GetListTasks(int projectId)
        {
            // 1. Find list task by id project
            var listTasks = await _projectRepository.GetListTasksByProjectId(projectId);

            // 2. Return message
            return new ProjectManagerResponse
            {
                IsSuccess = true,
                Message = "Get all list task of project",
                Project = listTasks,
            };
        }
    }
}
