using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<Domain.Entities.Task> FindByIdAsync(int taskId);
        Domain.Entities.Task FindByIdAndListTask(int taskId, int listTaskId);
    }
}
