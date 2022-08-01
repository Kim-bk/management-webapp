using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Service.DTOs.Responses;
using Service.DTOS.Responses;
using Service.Interfaces;

namespace Service
{
    public class ListTaskService : IListTaskService
    {
        private IListTaskRepository _listTaskRepository;
        private IUnitOfWork _unitOfWork;

        public ListTaskService(IListTaskRepository listTaskRepository, IUnitOfWork unitOfWork)
        {
            _listTaskRepository = listTaskRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<UserManagerResponse> AddTaskToList(string titleTask, int listTaskId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find list task by id
                var listTask = await _listTaskRepository.FindListTaskByIdAsync(listTaskId);

                // 2. Init object Task
                var task = new Domain.Entities.Task
                {
                    Title = titleTask,
                    ListTask = listTask
                };

                // 3. Add the task to list task found
                listTask.Tasks.Add(task);

                // 4. Commit 
                await _unitOfWork.CommitTransaction();

                // 5. Return message 
                return new UserManagerResponse
                {
                    Message = "Add task to " + "list task " + listTaskId.ToString(),
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

            // 2. Init list task (title, id) to store and return to client
            var storeTasks = new List<Domain.Entities.Task>();
            foreach (var listTask in getCollectionTasks)
            {
                foreach (var task in listTask)
                {
                    var t = new Domain.Entities.Task
                    {
                        Id = task.Id,
                        Title = task.Title,
                    };
                    storeTasks.Add(t);
                }
            }
            // 3. Return result
            return new ListTaskManagerResponse
            {
                Message = "Get all tasks in list success!",
                IsSuccess = true,
                Task = storeTasks
            };
        }
    }
}
