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
        private IProjectRepository _projectRepository;
        private IListTaskRepository _listTaskRepository;
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        public ProjectService(IProjectRepository projectRepository, IListTaskRepository listTaskRepository,
                            IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _projectRepository = projectRepository;
            _listTaskRepository = listTaskRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        
        public async Task<UserManagerResponse> AddMemberToProject(ProjectRequest model)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find user in Database
                var user = await _userManager.FindByIdAsync(model.UserId);

                // 2. Find project in Database
                var project = await _projectRepository.FindByIdAsync(model.ProjectId);

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

        public async Task<UserManagerResponse> CreateListTask(CommonRequest model)
        {
            try
            {
                // 1. Check if list task is duplicated
                var listTaskCheck = await _listTaskRepository.FindByNameAsync(model.Title);
                if (listTaskCheck != null)
                {
                    return new UserManagerResponse
                    {
                        Message = "The List Task already exists in the project",
                        IsSuccess = true,
                    };
                }
                await _unitOfWork.BeginTransaction();

                // 2.Find project by it ID
                var project = await _projectRepository.FindByIdAsync(model.Id);

                // 3. Create list task in the project then update
                var listTask = new ListTask
                {
                    Title = model.Title,
                    Project = project
                };

                project.ListTasks.Add(listTask);

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
                    IsSuccess = false
                };
            }
        }

        public async Task<UserManagerResponse> CreateUserProject(string userId, ProjectRequest model)
        {
            try
            {
                // 1. Check if name project is exist
                var project = _projectRepository.FindByNameAsync(model.Name);
                if (project != null)
                {
                    return new UserManagerResponse
                    {
                        Message = "The project already exists",
                        IsSuccess = true,
                    };
                }

                await _unitOfWork.BeginTransaction();

                // 2. Find user
                var user = await _userManager.FindByIdAsync(userId);

                // 3. Create Project
                var p = new Project
                {
                    Name = model.Name,
                };

                // 4. Add user to project
                p.Users.Add(user);
                _projectRepository.CreateProject(p);

                // 5. Commit transaction if not catch exception
                await _unitOfWork.CommitTransaction();

                // 6. Return message create success
                return new UserManagerResponse
                {
                    Message = "Create new project!",
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
