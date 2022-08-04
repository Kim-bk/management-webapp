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
      
        public async Task<TaskManagerResponse> GetTask(int taskId)
        {
            try
            {
                // 1. Get all information of task by id
                var task = await _taskRepository.GetByIdAsync(taskId);

                // 2. Return to client information of task
                return new TaskManagerResponse
                {
                    Message = "Get task",
                    Task = task,
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
        public async Task<UserManagerResponse> AddLabelToTask(LabelRequest model)
        {
           try
            {
                // 1. Check if duplicate name label
                var task = await _labelRepository.FindByNameAsync(model.Title);
                if (task != null)
                {
                    return new UserManagerResponse
                    {
                        Message = "The Label already exists in the task!",
                        IsSuccess = true,
                    };
                }

                await _unitOfWork.BeginTransaction();

                // 2. Init new Label
                var label = new Label
                {
                    Title = model.Title,
                    Color = model.Color
                };

                // 3. Find task by id 
                Domain.Entities.Task task = await _taskRepository.FindByIdAsync(model.TaskId);

                // 4. Update task with label
                task.Labels.Add(label);
                
                // 5. Commit changes
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

        public async Task<UserManagerResponse> RemoveLabelInTask(LabelRequest model)
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

        public async Task<TaskManagerResponse> AddToDoToTask(ToDoRequest model)
        {
            try
            {
                // 1. Check duplicate title todo parent
                if (model.ParentId != null)
                {
                    var isDuplicated = await _todoRepository.FindByNameAsync(model.Title);
                    if (isDuplicated != null)
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
                var task = await _taskRepository.FindByIdAsync(model.TaskId);

                // 3. Init entity Todo
                var todo = new Todo
                {
                    Title = model.Title,
                    IsDone = false,
                    ParentId = model.ParentId,
                    Task = task,
                };

                // 4. Add todo to task
                task.Todos.Add(todo);

                // 5. Commit
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
