using System;
using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System.Linq;
using API.Services.Interfaces;
using API.DTOs;
using System.Collections.Generic;
using API.Services;

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
        ~TaskService()
        {
            _userManager.Dispose();
        }
      
        public async Task<TaskManagerResponse> GetTask(int taskId)
        {
            try
            {
                // 1. Get all information of task by id
                var task = await _taskRepository.FindByIdAsync(taskId);

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
                return new TaskManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = true
                };
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
                    return new UserManagerResponse
                    {
                        Message = "The Label already exists!",
                        IsSuccess = true,
                    };
                }

                await _unitOfWork.BeginTransaction();

                // 2. Find task by id 
                Domain.Entities.Task task = await _taskRepository.FindByIdAsync(request.TaskId);

                // 3. Update task with new label 
                task.Labels.Add(new Label 
                { 
                    Title = request.Title,
                    Color = request.Color
                });
                
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
                return new UserManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = true,
                };
            }
        }

        public async Task<UserManagerResponse> RemoveLabelInTask(LabelRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find label by Id
                var label = await _labelRepository.FindByIdAsync(request.Id);

                // 2. Find task by Id
                var task = await _taskRepository.FindByIdAsync(request.TaskId);

                // 3. Remove label in task then update
                task.Labels.Remove(label);

                // 4. Commit
                await _unitOfWork.CommitTransaction();

                // 5. Return message
                return new UserManagerResponse
                {
                    Message = "Remove label " + label.Title + " in task " + task.Title,
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
                        return new TaskManagerResponse
                        {
                            Message = "The To-do already exists in the task!",
                            IsSuccess = true
                        };
                    }
                }

                await _unitOfWork.BeginTransaction();

                // 2. Find task by ID
                var task = await _taskRepository.FindByIdAsync(request.TaskId);

                // 3. Add new todo to task
                task.Todos.Add(new Todo 
                {
                    Title = request.Title,
                    IsDone = false,
                    ParentId = request.ParentId,
                    Task = task,
                });

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
                return new TaskManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = true,
                };
            }
        }
        public async Task<TaskManagerResponse> ManageToDoItems(ToDoRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find to do item by id and task id
                var todoItem = _todoRepository.FindToDoItems(request.TaskId, request.Id);

                // 2. Validate whether todo is parent or not
                if (todoItem.ParentId == null)
                {
                    return new TaskManagerResponse
                    {
                        Message = "Todo Parent can not check or uncheck!",
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
                await _unitOfWork.RollbackTransaction();
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

                // 3. Swap and update
                var tempPos = firstTask.Position;
                var tempList = firstTask.ListTaskId;

                firstTask.Position = secondTask.Position;
                firstTask.ListTaskId = secondTask.ListTaskId;

                secondTask.Position = tempPos;
                secondTask.ListTaskId = tempList;

                await _unitOfWork.CommitTransaction();

                return new UserManagerResponse
                {
                    Message = "Swap success!",
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
        private int FindMaxPosition(ICollection<Domain.Entities.Task> list)
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

                // 2. Set max position for task in list
                var positon = FindMaxPosition(listTask.Tasks) + 1;

                // 3. Init object Task
                var task = new Domain.Entities.Task
                {
                    Title = request.Title,
                    ListTask = listTask,
                    Position = positon
                };

                // 4. Add the task to list task found
                listTask.Tasks.Add(task);

                // 5. Commit 
                await _unitOfWork.CommitTransaction();

                // 6. Return message 
                return new UserManagerResponse
                {
                    Message = "Add task " + task.Title.ToString() + " to " + "list task " + task.ListTask.Title.ToString(),
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                return new UserManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = false,
                };
            }
        }
    }
}
