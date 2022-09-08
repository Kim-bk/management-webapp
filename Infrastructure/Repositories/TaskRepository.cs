using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using System.Linq;
using TaskEntity = Domain.AggregateModels.TaskAggregate.Task;

namespace Infrastructure.Repositories
{
    public class TaskRepository : Repository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
