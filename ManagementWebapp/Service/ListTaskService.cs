using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Service.DTOs.Responses;
using Service.DTOS.Responses;
using Service.Interfaces;
using System.Linq;
using Service.DTOs.Requests;

namespace Service
{
    public class ListTaskService : IListTaskService
    {
        private IListTaskRepository _listTaskRepository;
        private ITaskRepository _taskRepository;
        private IUnitOfWork _unitOfWork;

        public ListTaskService(IListTaskRepository listTaskRepository, IUnitOfWork unitOfWork, ITaskRepository taskRepository)
        {
            _listTaskRepository = listTaskRepository;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }
        private int FindMaxPosition(ICollection<Domain.Entities.Task> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
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

        public async Task<UserManagerResponse> AddTaskToList(string titleTask, int listTaskId, string userId)
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
                    Title = titleTask,
                    ListTask = listTask,
                    UserId = userId,         // user create  
                    Position = positon
                };

                // 4. Add the task to list task found
                listTask.Tasks.Add(task);

                // 5. Commit 
                await _unitOfWork.CommitTransaction();

                // 6. Return message 
                return new UserManagerResponse
                {
                    Message = "Add task " + task.Id.ToString() + " to " + "list task " + listTaskId.ToString(),
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

            var findListTask = await _listTaskRepository.FindListTaskByIdAsync(listTaskId);
        
            if (findListTask == null)
            {
                return new ListTaskManagerResponse
                {
                    Message = "List Task is not found!",
                    IsSuccess = true,
                };
            }

            // 2. Get all tasks in list task by ID
            var getCollectionTasks = _listTaskRepository.GetAllTasks(listTaskId);

            // 3. Init list task (title, id) to store and return to client
            var storeTasks = new List<Domain.Entities.Task>();
            foreach (var listTask in getCollectionTasks)
            {
                foreach (var task in listTask)
                {
                    var t = new Domain.Entities.Task
                    {
                        Id = task.Id,
                        Title = task.Title,
                        ListTaskId = task.ListTaskId,
                        UserId = task.UserId,
                        Position = task.Position
                    };
                    storeTasks.Add(t);
                }
            }
            // 4. Return result
            return new ListTaskManagerResponse
            {
                Message = "Get all tasks in list success!",
                IsSuccess = true,

                // 5. Get all task ordered by ascending to show in list task
                Task = storeTasks.OrderBy(t => t.Position).ToList()
            };
        }

        public async Task<UserManagerResponse> MoveTask(TaskRequest model)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find each task in list task by list task id and task id
                var firstTask = _taskRepository.FindByTwoId(model.BeforeTaskId, model.BeforeListId);
                var secondTask = _taskRepository.FindByTwoId(model.AfterTaskId, model.AfterListId);

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
                    IsSuccess = false,
                };
            }

        }
    }
}
