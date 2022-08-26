using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        void CreateTask(AggregateModels.TaskAggregate.Task task);
        Task<AggregateModels.TaskAggregate.Task> FindByIdAsync(int taskId);
        AggregateModels.TaskAggregate.Task FindByIdAndListTask(int taskId, int listTaskId);
        void DeleteTask(AggregateModels.TaskAggregate.Task task);
    }
}
