using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IListTaskRepository
    {
        void Create(ListTask listTask);
        Task<ListTask> FindListTaskByIdAsync(int listTaskId);
        Task<List<Domain.Entities.Task>> GetTasksInList(int listTaskId);
        Task<ListTask> FindByNameAsync(string nameListTask);
    }
}
