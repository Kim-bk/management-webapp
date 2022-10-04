using TaskEntity = Domain.AggregateModels.TaskAggregate.Task;

namespace Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IRepository<TaskEntity>
    {
    }
}
