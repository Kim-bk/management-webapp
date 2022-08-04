using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IListTaskRepository
    {
        void Create(ListTask listTask);
        Task<ListTask> FindListTaskByIdAsync(int listTaskId);
        List<TaskDTO> GetAllTasks(ListTask findListTask);
        Task<ListTask> FindByNameAsync(string nameListTask);
    }
}
