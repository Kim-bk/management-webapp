using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Accounts;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.DTOS.Requests;
using Service.DTOS.Responses;
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

        public async Task<UserManagerResponse> AddMemberToProject(string userId, int projectId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find user in Database
                var user = await _userManager.FindByIdAsync(userId);

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

        public async Task<UserManagerResponse> CreateListTask(string title, int projectId)
        {
            try
            {
                // 1. Validate
                await _unitOfWork.BeginTransaction();

                // 2.Find project by it ID
                var project = await _projectRepository.FindByIdAsync(projectId);

                // 3. Create list task in the project then update
                var listTask = new ListTask
                {
                    Title = title,
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
                await _unitOfWork.BeginTransaction();

                // 1. Find user
                var user = await _userManager.FindByIdAsync(userId);

                // 2. Create Project
                var p = new Project
                {
                    Name = model.Name,
                };

                // 3. Add user to project
                p.Users.Add(user);
                _projectRepository.CreateProject(p);

                // 4. Commit transaction if not catch exception
                await _unitOfWork.CommitTransaction();

                // 5. Return message create success
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
    }
}
