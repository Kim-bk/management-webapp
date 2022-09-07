using System;
using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;
using API.Services;
using API.Services.Interfaces;
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System.Linq;
using System.Data.Entity.Core;
using API.DTOs;

namespace Service
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IListTaskRepository _listTaskRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProjectService(IProjectRepository projectRepository, IListTaskRepository listTaskRepository,
                            IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapperCustom mapper)
                            : base(unitOfWork, mapper)
        {
            _projectRepository = projectRepository;
            _listTaskRepository = listTaskRepository;
            _userManager = userManager;
        }
        
        public async Task<UserManagerResponse> AddMemberToProject(int projectId, ProjectRequest model)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find project in Database
                var project = await _projectRepository.FindAsync(p => p.Id == projectId);

                // 2. Find user
                var user = await _userManager.FindByIdAsync(model.UserId);

                // 3. Add project default to user then update
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
        public async Task<ListTaskManagerResponse> GetListTask(int listTaskId)
        {
            try
            {
                // 1. Validate
                if (listTaskId == 0)
                {
                    throw new NullReferenceException("List Task Id is null!");
                }

                var listTask = await _listTaskRepository.FindAsync(lt => lt.ListTaskId == listTaskId);
                if (listTask == null)
                {
                    throw new ObjectNotFoundException("The list task is not found!");
                }

                // 2. Return result
                return new ListTaskManagerResponse
                {
                    Message = "Get all tasks in list success!", 
                    IsSuccess = true,
                    ProjectId = Convert.ToInt32(listTask.ProjectId),
                    Task = _mapper.MapTasks(listTask.Tasks.ToList()).OrderBy(t => t.Position).ToList()
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<UserManagerResponse> CreateListTask(ListTaskRequest model)
        {
            try
            {
                // 1.Find project by it ID
                var project = await _projectRepository.FindAsync(p => p.Id == model.ProjectId);
                if (project == null)
                {
                    throw new ObjectNotFoundException("The project is not found!");
                }

                // 2. Check if list task is duplicated
                var listTask = await _listTaskRepository.FindAsync(lt => lt.Title == model.Title);
                if (listTask != null && listTask.ProjectId == model.ProjectId)
                {
                    throw new ArgumentException("Project already exists!");
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
                var project = await _projectRepository.FindAsync(p => p.Name == model.Name);
             
                // 2. Check if name project is exist in user projects
                if (project != null && project.Users
                    .Contains(project.Users.FirstOrDefault(u => u.Id == userId)))
                {
                    throw new ArgumentException("Project already exists!");
                }

                await _unitOfWork.BeginTransaction();

                // 4. Create Project then add member
                var createProject = new Project(model.Name);
                _projectRepository.AddAsync(createProject);

                // 5. Find user by id then add project to user
                var user = await _userManager.FindByIdAsync(userId);
                createProject.AddMember(user);

                // 6. Commit transaction if not catch exception
                await _unitOfWork.CommitTransaction();

                // 7. Return message create success
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

        public async Task<ProjectManagerResponse> GetProject(int projectId)
        {
            // 1. Find project
            var project = await _projectRepository.FindAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new ObjectNotFoundException("Project cant be found!");
            }

            // 2. Return message
            return new ProjectManagerResponse
            {
                IsSuccess = true,
                Message = "Get all list task of project",
                ListTasks = _mapper.MapListTasks(project.ListTasks.ToList()),
            };
        }
        public async Task<UserManagerResponse> DeleteListTask(int projectId, int listTaskId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find project
                var project = await _projectRepository.FindAsync(p => p.Id == projectId);

                // 2. Validate if the list task is in project
                if (project == null)
                {
                    throw new ObjectNotFoundException("The project is not found!");
                }

                if (!project.ListTasks.Contains(
                    project.ListTasks.FirstOrDefault(lt => lt.ListTaskId == listTaskId)))
                {
                    // 3. Return error cant find list task in the project
                    throw new ObjectNotFoundException("List task is not found in project!");
                }

                // 4. Send event (ListTaskDeletedDomainEvent) to delete all tasks in list
                // this will be handled in ListTaskDeletedDomainEvenHandler
                project.DeleteListTask(listTaskId);

                await _unitOfWork.CommitTransaction();

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

        public async Task<ProjectManagerResponse> DeleteProject(int projectId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find project
                var project = await _projectRepository.FindAsync(p => p.Id == projectId);
                if (project == null)
                {
                    throw new ObjectNotFoundException("The project is not found!");
                }

                // 2. Delete project
                project.DeleteProject();
                _projectRepository.Delete(project);

                // 3. Save changes
                await _unitOfWork.CommitTransaction();
                return new ProjectManagerResponse
                {
                    IsSuccess = true,
                    Message = "Delete project " + project.Name,
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw e;
            }
        }
    }
}
