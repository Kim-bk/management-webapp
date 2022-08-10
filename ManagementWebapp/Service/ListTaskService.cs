using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Service.Interfaces;
using System.Linq;
using Domain.DTOs.Responses;
using Domain.DTOs.Requests;

namespace Service
{
    public class ListTaskService : IListTaskService
    {
        private readonly IListTaskRepository _listTaskRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ListTaskService(IListTaskRepository listTaskRepository, IUnitOfWork unitOfWork,
                            ITaskRepository taskRepository)
        {
            _listTaskRepository = listTaskRepository;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
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

        public async Task<UserManagerResponse> AddTaskToList(int listTaskId, CommonRequest model, string userId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find list task by ID
                var listTask = await _listTaskRepository.FindListTaskByIdAsync(listTaskId);

                // 2. Set max position for task in list
                var positon = FindMaxPosition(listTask.Tasks) + 1;

                // 3. Init object Task
                var task = new Domain.Entities.Task
                {
                    Title = model.Title,
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

        public async Task<ListTaskManagerResponse> GetAllTasks(int listTaskId)
        {
            // 1. Validate
            if (listTaskId == 0)
            {
                return new ListTaskManagerResponse
                {
                    Message = "List Task Id is null",
                    IsSuccess = true,
                };
            }

            // 2. Get all tasks in list task
            var result = await _listTaskRepository.GetTasksInList(listTaskId);

            // 3. Validate if list task not found
            if (result == null)
            {
                return new ListTaskManagerResponse
                {
                    Message = "List Task is not found!",
                    IsSuccess = true,
                };
            }

            // 4. Return result
            return new ListTaskManagerResponse
            {
                Message = "Get all tasks in list success!",
                IsSuccess = true,
                Task = result
            };
        }
        public async Task<UserManagerResponse> MoveTask(TaskRequest model)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find each task in list task by list task id and task id
                var firstTask = _taskRepository.FindByIdAndListTask(model.BeforeTaskId, model.BeforeListId);
                var secondTask = _taskRepository.FindByIdAndListTask(model.AfterTaskId, model.AfterListId);

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
    }
}
