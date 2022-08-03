using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.DTOs.Requests;
using Service.DTOs.Responses;
using Service.DTOS.Responses;
using Service.Interfaces;

namespace Service
{
    public class TaskService : ITaskService
    {
        private ITaskRepository _taskRepository;
        private IToDoRepository _todoRepository;
        private ILabelRepository _labelRepository;
        private UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;
        public TaskService(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ILabelRepository labelRepository, 
                        IToDoRepository todoRepository, UserManager<ApplicationUser> userManager)
        {
            _labelRepository = labelRepository;
            _todoRepository = todoRepository;
            _taskRepository = taskRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        } 
        private List<Todo> GetAllTodos(Domain.Entities.Task task)
        {
            // 1. Init list labels to store label of the task
            var storeTodos = new List<Todo>();
            foreach (var todoItem in task.Todos)
            {
                var t = new Todo
                {
                    Id = todoItem.Id,
                    Title = todoItem.Title,
                    IsDone = todoItem.IsDone,
                    ParentId = todoItem.ParentId,
                };
                storeTodos.Add(t);
            }
            // 2. Return result
            return storeTodos;
        }

        private List<ApplicationUser> GetAllMembersAssigned(Domain.Entities.Task task)
        {
            // 1. Init list labels to store label of the task
            var storeMembers = new List<ApplicationUser>();
            foreach (var member in task.Users)
            {
                var m = new ApplicationUser
                {
                    Id = member.Id,
                    UserName = member.UserName,
                };
                storeMembers.Add(m);
            }
            // 2. Return result
            return storeMembers;
        }
        private List<Label> GetAllLabels(Domain.Entities.Task task)
        {
            // 1. Init list labels to store label of the task
            var storeLabels = new List<Label>();
            foreach (var label in task.Labels)
            {
                var l = new Label
                {
                    Id = label.Id,
                    Title = label.Title,
                    Color = label.Color,
                };
                storeLabels.Add(l);
            }
            // 2. Return result
            return storeLabels;
        }
        public async Task<TaskManagerResponse> GetAllComponentsOfTask(int taskId)
        {
            try
            {
                // 1. Find task by id
                var task = await _taskRepository.FindByIdAsync(taskId);

                // 2. Get labels of task
                var listLabels = GetAllLabels(task);

                // 3. Get todos of task
                var listTodos = GetAllTodos(task);

                // 4. Get members assigned in task
                var listMembers = GetAllMembersAssigned(task);

                // 5. Return for client information of task
                return new TaskManagerResponse
                {
                    Message = "Get all components of task",
                    Title = task.Title,
                    Todos = listTodos,
                    Labels = listLabels,
                    Members = listMembers,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new TaskManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = false
                };
            }
        }
        public async Task<UserManagerResponse> AddLabel(LabelRequest model)
        {
           try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Init new Label
                var label = new Label
                {
                    Title = model.Title,
                    Color = model.Color
                };

                // 2. Find task by id 
                Domain.Entities.Task task = await _taskRepository.FindByIdAsync(model.TaskId);

                // 3. Update task with label
                task.Labels.Add(label);

                // 4. Commit
                await _unitOfWork.CommitTransaction();
                return new UserManagerResponse
                {
                    Message = "Add label " + label.Title.ToString() + " to task " + task.Title.ToString(),
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new UserManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = true,
                };
            }
        }

        public async Task<UserManagerResponse> RemoveLabel(LabelRequest model)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find label by Id
                var label = await _labelRepository.FindByIdAsync(model.Id);

                // 2. Find task by Id
                var task = await _taskRepository.FindByIdAsync(model.TaskId);

                // 3. Remove label in task then update
                task.Labels.Remove(label);

                // 4. Commit
                await _unitOfWork.CommitTransaction();

                // 5. Return message
                return new UserManagerResponse
                {
                    Message = "Remove label " + label.Title + " from task " + task.Title,
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

        public async Task<TaskManagerResponse> AddToDo(ToDoRequest model)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find task by ID
                var task = await _taskRepository.FindByIdAsync(model.TaskId);

                // 2. Init object Todo
                var todo = new Todo
                {
                    Title = model.Title,
                    IsDone = false,
                    ParentId = model.ParentId,
                    Task = task,
                };

                // 3. Add todo to task
                task.Todos.Add(todo);

                // 4. Commit
                await _unitOfWork.CommitTransaction();

                return new TaskManagerResponse
                {
                    Message = "Add to do " + todo.Title + " to task " + task.Title,
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                return new TaskManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = true,
                };
            }
        }

        public async Task<TaskManagerResponse> ManageToDoItems(ToDoRequest model)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find to do item by id and task id
                var todoItem = _todoRepository.FindToDoItems(model.TaskId, model.Id);

                // 2. Validate whether todo is parent or not
                if (todoItem.ParentId == null)
                {
                    return new TaskManagerResponse
                    {
                        Message = "ToDo Parent can not check or uncheck!",
                        IsSuccess = true
                    };
                }

                // 3. Update status check / uncheck of todo item
                if (Convert.ToBoolean(todoItem.IsDone))
                {
                    todoItem.IsDone = false;
                }
                else
                {
                    todoItem.IsDone = true;
                }

                // 4. Commit changes
                await _unitOfWork.CommitTransaction();
                
                return new TaskManagerResponse
                {
                    Message = "Success check/uncheck todo item",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new TaskManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = true
                };
            }
        }

        public async Task<TaskManagerResponse> AssignMember(int taskId, string userId)
        {
            // 1. Validate input
            if (taskId == 0 || userId == null)
            {
                return new TaskManagerResponse
                {
                    Message = "Empty input!!",
                    IsSuccess = true,
                };
            }

            try
            {
                await _unitOfWork.BeginTransaction();
                // 2. Find user by id
                var user = await _userManager.FindByIdAsync(userId);

                // 3. Find task by id
                var task = await _taskRepository.FindByIdAsync(taskId);

                // 4. Update member into task and assign user
                task.Users.Add(user);
                task.DoingId = userId;

                // 5. Commit changes
                await _unitOfWork.CommitTransaction();

                return new TaskManagerResponse
                {
                    Message = "Assign user " + user.UserName + " to task " + task.Title,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                return new TaskManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = true
                };
            }
        }
    }
}
