using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Service.Interfaces;
using System.Linq;
using API.DTOs.Responses;
using API.DTOs.Requests;
using API.Services.Interfaces;
using API.Services;

namespace Service
{
    public class ListTaskService : BaseService, IListTaskService
    {
        private readonly IListTaskRepository _listTaskRepository;
        private readonly ITaskRepository _taskRepository;

        public ListTaskService(IListTaskRepository listTaskRepository, IUnitOfWork unitOfWork,
                            ITaskRepository taskRepository, IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _listTaskRepository = listTaskRepository;
            _taskRepository = taskRepository;
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

            var listTask = await _listTaskRepository.FindListTaskByIdAsync(listTaskId);
            if (listTask == null)
            {
                return new ListTaskManagerResponse
                {
                    Message = "List Task Id is not found!",
                    IsSuccess = true,
                };
            }

            // 2. Get all tasks in list task
            var tasks = await _listTaskRepository.GetTasksInList(listTaskId);

            // 3. Return result
            return new ListTaskManagerResponse
            {
                Message = "Get all tasks in list success!",
                IsSuccess = true,
                Task = _mapper.MapTasks(tasks).OrderBy(t => t.Position).ToList()
            };
        }
    }
}
