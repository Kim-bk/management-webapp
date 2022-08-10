﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IListTaskRepository
    {
        void Create(ListTask listTask);
        Task<ListTask> FindListTaskByIdAsync(int listTaskId);
        Task<List<TaskDTO>> GetTasksInList(int listTaskId);
        Task<ListTask> FindByNameAsync(string nameListTask);
    }
}
