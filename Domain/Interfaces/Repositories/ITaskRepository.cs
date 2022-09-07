using System.Threading.Tasks;
using TaskEntity = Domain.AggregateModels.TaskAggregate.Task;

namespace Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IRepository<TaskEntity>
    {
        TaskEntity FindByIdAndListTask(int taskId, int listTaskId);
    }
}
