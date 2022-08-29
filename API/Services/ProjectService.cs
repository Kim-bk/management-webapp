﻿using System;
using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;
using API.Services;
using API.Services.Interfaces;
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;
using Domain.DomainEvents;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System.Linq;

namespace Service
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IListTaskRepository _listTaskRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProjectService(IProjectRepository projectRepository, IListTaskRepository listTaskRepository,
                            IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapperCustom mapper,
                            ITaskService taskService, ITaskRepository taskRepository)
                            : base(unitOfWork, mapper)
        {
            _projectRepository = projectRepository;
            _listTaskRepository = listTaskRepository;
            _taskRepository = taskRepository;
            _userManager = userManager;
        }
        ~ProjectService()
        {
            _userManager.Dispose();
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
                project.AddMember(user);

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
                throw e;
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
                project.CreateListTask(model.Title);
                
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
                throw e;
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

                // 4. Create Project then add member
                var newProject = new Project(model.Name);
                newProject.AddMember(user);

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
                throw e;
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
                ListTasks = _mapper.MapListTasks(listTasks),
            };
        }
        public async Task<UserManagerResponse> DeleteListTask(int projectId, int listTaskId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                // 1. Find project
                var project = await _projectRepository.FindByIdAsync(projectId);

                // 2. Validate if the list task is in project
                if (project == null)
                {
                    return new UserManagerResponse
                    {
                        Message = "Project is not found in the project !",
                        IsSuccess = true,
                    };
                }

                if (!project.ListTasks.Contains(
                    project.ListTasks.FirstOrDefault(lt => lt.ListTaskId == listTaskId)))
                {
                    // Return error cant find list task in the project
                    return new UserManagerResponse
                    {
                        Message = "List Task is not found in the project !",
                        IsSuccess = true,
                    };
                }

                // 2. Send event (ListTaskDeletedDomainEvent) to delete all tasks in list
                // this will be handled in ListTaskDeletedDomainEvenHandler
                project.DeleteListTask(listTaskId);

                await _unitOfWork.SaveEntitiesAsync();

                return new UserManagerResponse
                {
                    Message = "List Task " + " have been deleted!",
                    IsSuccess = true,
                };
            }
            catch(Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw e;
            }
           
        }

        public async Task<ProjectManagerResponse> GetAllProjects()
        {
            var listProject = await _projectRepository.GetAll();
            return new ProjectManagerResponse
            { 
                Projects = _mapper.MapProject(listProject),
                IsSuccess = true
            };

        }
    }
}
