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
using TaskEntity = Domain.AggregateModels.TaskAggregate.Task;
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
                var task = await _taskRepository.FindAsync(t => t.Id == taskId);

                // 2. Check if task is existed
                if (task == null || task.ListTaskId == null)
                {
                    throw new ArgumentNullException("Task is not found!");
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
                var label = await _labelRepository.FindAsync(lb => lb.Title == request.Title);
                if (label != null)
                {
                    throw new ArgumentException("Duplicated label!");
                }

                await _unitOfWork.BeginTransaction();

                // 2. Find task by id 
                var task = await _taskRepository.FindAsync(t => t.Id == request.TaskId);

                // 3. Update task with new label 
                task.AddLabel(request.Title, request.Color);

                // 4. Commit changes
                await _unitOfWork.CommitTransaction();

                return new UserManagerResponse
                {
                    Message = "Add label " + request.Title.ToString() + " to task " + task.Title.ToString(),
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
                // 1. Find task by Id
                var task = await _taskRepository.FindAsync(t => t.Id == request.TaskId);

                // 2. Remove label in task
                task.RemoveLabel(request.Id);

                await _unitOfWork.CommitTransaction();

                return new UserManagerResponse
                {
                    Message = "Remove label " + request.Id + " in task " + task.Title,
                };
            }
            catch (Exception e)
            {
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
                    var parenTodo = await _todoRepository.FindAsync(td => td.Title == request.Title);
                    if (parenTodo != null && parenTodo.TaskId == request.TaskId)
                    {
                        throw new ArgumentException("Duplicate to-do!");
                    }
                }

                await _unitOfWork.BeginTransaction();

                // 2. Find task by ID
                var task = await _taskRepository.FindAsync(t => t.Id == request.TaskId);

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
        public async Task<TaskManagerResponse> UpdateTodoItems(ToDoRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find to do item by id and task id
                var todoItem = await _todoRepository.FindAsync(
                    td => td.Id == request.Id && td.TaskId == request.TaskId);

                // 2. Validate
                if (todoItem == null)
                {
                    throw new ArgumentNullException("Todo not found!");
                }

                // 3. Validate whether todo is parent or not
                if (todoItem.ParentId == null)
                {
                    throw new ArgumentException("To-do parent cant not checked or unchecked!");
                }

                // 3. Update Todo item
                todoItem.Task.UpdateTodoItem(todoItem, request);

                // 4. Commit changes
                await _unitOfWork.CommitTransaction();
                
                return new TaskManagerResponse
                {
                    Message = "Success check/uncheck todo item",
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
                throw new ArgumentNullException("Input can not be null!");
            }

            try
            {
                await _unitOfWork.BeginTransaction();

                // 2. Find user by id
                var user = await _userManager.FindByIdAsync(userId);

                // 3. Find task by id
                var task = await _taskRepository.FindAsync(t => t.Id == taskId);

                // 4. Validate
                if (task == null || task.ListTaskId == null)
                {
                    throw new ObjectNotFoundException("Task cant be found!");
                }

                if (user == null)
                {
                    throw new ObjectNotFoundException("User cant be found!");
                }

                // 5. Assing user to task
                task.AssignMember(user);

                // 6. Commit changes
                await _unitOfWork.CommitTransaction();

                return new TaskManagerResponse
                {
                    Message = "Assign user " + user.UserName + " to task " + task.Title,
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
                var firstTask = await _taskRepository.FindAsync(t => t.Id == taskId);
                var secondTask = await _taskRepository.FindAsync(t => t.Id == request.TaskId);

                // 2. Validate
                if (firstTask == null || secondTask == null
                     || firstTask.ListTaskId == null || secondTask.ListTaskId == null)
                {
                    throw new ArgumentNullException("Cant find one of these task!");
                }

                // 3. Swap position of both
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
        private int FindMaxPosition(List<TaskEntity> tasks)
        {
            if (tasks.Count == 0)
            {
                return 0;
            }
            int currentPos = int.MinValue;
            foreach (var task in tasks)
            {
                if (task.Position > currentPos)
                {
                    currentPos = Convert.ToInt32(task.Position);
                }
            }
            return currentPos + 1; // this is max pos
        }
        public async Task<UserManagerResponse> CreateTask(TaskRequest request, string userId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find list task
                var listTask = await _listTaskRepository.FindAsync(lt => lt.ListTaskId == request.ListTaskId);
                
                // 2. Validate
                if (listTask != null)
                {
                    throw new ArgumentException("List task cant be found!");
                }

                // 3. Set max position (default) for task in list
                var positon = FindMaxPosition(listTask.Tasks.ToList());

                // 4. Init object Task
                var task = new TaskEntity(request.Title, listTask, positon);
                _taskRepository.AddAsync(task);
                await _unitOfWork.CommitTransaction();

                return new UserManagerResponse
                {
                    Message = "Add task " + task.Title.ToString() + " to " + "list task " + task.ListTask.Title.ToString(),
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
