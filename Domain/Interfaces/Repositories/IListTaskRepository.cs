using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.AggregateModels.ProjectAggregate;

namespace Domain.Interfaces.Repositories
{
    public interface IListTaskRepository
    {
        void Create(ListTask listTask);
        Task<ListTask> FindListTaskByIdAsync(int listTaskId);
        Task<ListTask> FindByNameAsync(string nameListTask);
        void DelteListTask(ListTask listTask);
    }
}
