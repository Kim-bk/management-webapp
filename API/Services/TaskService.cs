using System;
using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System.Linq;
using API.Services.Interfaces;
using API.DTOs;
using System.Collections.Generic;
using API.Services;
using Domain.AggregateModels.UserAggregate;
using System.Data.Entity.Core;

namespace Service
{
    public class TaskService : BaseService, ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IToDoRepository _todoRepository;
        private readonly ILabelRepository _labelRepository;
        private readonly IListTaskRepository _listTaskRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public TaskService(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ILabelRepository labelRepository,
                        IToDoRepository todoRepository, UserManager<ApplicationUser> userManager,
                        IMapperCustom mapper, IListTaskRepository listTaskRepository) : base(unitOfWork, mapper)
        {
            _labelRepository = labelRepository;
            _todoRepository = todoRepository;
            _taskRepository = taskRepository;
            _listTaskRepository = listTaskRepository;
            _userManager = userManager;
        } 
        public async Task<TaskManagerResponse> GetTask(int taskId)
        {
            try
            {
                // 1. Get all information of task by id
                var task = await _taskRepository.FindByIdAsync(taskId);

                // 2. Check if task is existed
                if (task == null)
                {
                    throw new ObjectNotFoundException("Task is not found!");

                }

                // 2. Map Task to Task DTO
                var taskDTO = new TaskDTO
                {
                    TaskId = task.Id,
                    Position = task.Position,
                    Title = task.Title,
                    Todos = _mapper.MapTodos(task.Todos.ToList()),
                    Labels = _mapper.MapLabels(task.Labels.ToList()),
                    Members = _mapper.MapMembers(task.Users.ToList()),
                };

                return new TaskManagerResponse
                {
                    Message = "Get task",
                    Task = taskDTO,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<UserManagerResponse> AddLabelToTask(LabelRequest request)
        {
           try
            {
                // 1. Check if duplicate name label
                var label = await _labelRepository.FindByNameAsync(request.Title);
                if (label != null)
                {
                    throw new ArgumentException("Duplicate label!");
                }

                await _unitOfWork.BeginTransaction();

                // 2. Find task by id 
                Domain.AggregateModels.TaskAggregate.Task task = await _taskRepository.FindByIdAsync(request.TaskId);

                // 3. Update task with new label 
                task.AddLabel(request.Title, request.Color);

                // 4. Commit changes
                await _unitOfWork.CommitTransaction();

                return new UserManagerResponse
                {
                    Message = "Add label " + request.Title.ToString() + " to task " + task.Title.ToString(),
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw e;
            }
        }

        public async Task<UserManagerResponse> RemoveLabelInTask(LabelRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find task by Id
                var task = await _taskRepository.FindByIdAsync(request.TaskId);

                // 2. Remove label in task
                task.RemoveLabel(request.Id);

                await _unitOfWork.CommitTransaction();

                return new UserManagerResponse
                {
                    Message = "Remove label " + request.Id + " in task " + task.Title,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw e;
            }
        }
        public async Task<TaskManagerResponse> AddToDoToTask(ToDoRequest request)
        {
            try
            {
                // 1. Check duplicate title todo parent
                if (request.ParentId != null)
                {
                    var parenTodo = await _todoRepository.FindByNameAsync(request.Title);
                    if (parenTodo != null && parenTodo.TaskId == request.TaskId)
                    {
                        throw new ArgumentException("Duplicate to-do!");
                    }
                }

                await _unitOfWork.BeginTransaction();

                // 2. Find task by ID
                var task = await _taskRepository.FindByIdAsync(request.TaskId);

                // 3. Add new todo to task
                task.AddTodo(request.Title, request.ParentId);

                // 4. Commit
                await _unitOfWork.CommitTransaction();

                return new TaskManagerResponse
                {
                    Message = "Add to do " + request.Title + " to task " + request.Title,
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw e;
            }
        }
        public async Task<TaskManagerResponse> ManageToDoItems(ToDoRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find to do item by id and task id
                var task = await _taskRepository.FindByIdAsync(request.TaskId);
                var todoItem = task.GetToDoItemInTask(request.Id);

                // 2. Validate whether todo is parent or not
                if (todoItem.ParentId == null)
                {
                    throw new ArgumentException("To-do parent cant not checked or unchecked!");
                }

                // 3. Update status IsDone check / uncheck of todo item
                task.UpdateStatusTodoItem(todoItem);

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
                await _unitOfWork.RollbackTransaction();
                throw e;
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
                task.AssignMember(user);

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
                throw e;
            }
        }

        public async Task<UserManagerResponse> MoveTask(int taskId, MoveTaskRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find each task in list task by list task id and task id
                var firstTask = _taskRepository.FindByIdAndListTask(taskId, request.CurrentListId);
                var secondTask = _taskRepository.FindByIdAndListTask(request.TaskId, request.AfterListId);

                // 2. Validate
                if (firstTask == null || secondTask == null)
                {
                    return new UserManagerResponse
                    {
                        Message = "Can not find one of the task by id and list task id!",
                        IsSuccess = false,
                    };
                }
                firstTask.Swap(secondTask);
                await _unitOfWork.CommitTransaction();

                return new UserManagerResponse
                {
                    Message = "Swap success!",
                    IsSuccess = true,
                };
            }

            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw e;
            }
        }
        private int FindMaxPosition(List<Domain.AggregateModels.TaskAggregate.Task> list)
        {
            if (list.Count == 0)
            {
                return 0;
            }
            int maxPos = int.MinValue;
            foreach (var type in list)
            {
                if (type.Position > maxPos)
                {
                    maxPos = Convert.ToInt32(type.Position);
                }
            }
            return maxPos;
        }
        public async Task<UserManagerResponse> CreateTask(TaskRequest request, string userId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find list task by ID
                var listTask = await _listTaskRepository.FindListTaskByIdAsync(request.ListTaskId);
                
                // 2. Validate
                if (listTask != null && listTask.Tasks.Contains(
                    listTask.Tasks.FirstOrDefault(t => t.Title == request.Title)))
                {
                    return new UserManagerResponse
                    { 
                        Message = "Task is duplicated !",
                        IsSuccess = true
                    };
                }

                // 3. Set max position (default) for task in list
                var positon = FindMaxPosition(listTask.Tasks.ToList()) + 1;

                // 4. Init object Task
                var task = new Domain.AggregateModels.TaskAggregate.Task(request.Title, listTask, positon);
                _taskRepository.CreateTask(task);
                await _unitOfWork.CommitTransaction();

                return new UserManagerResponse
                {
                    Message = "Add task " + task.Title.ToString() + " to " + "list task " + task.ListTask.Title.ToString(),
                    IsSuccess = true,
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
