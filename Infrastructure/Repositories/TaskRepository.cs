using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TaskRepository : Repository<Domain.AggregateModels.TaskAggregate.Task>, ITaskRepository
    {
        public TaskRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<Domain.AggregateModels.TaskAggregate.Task> FindByIdAsync(int taskId)
        {
            return await DbSet.FindAsync(taskId);
        }

        public Domain.AggregateModels.TaskAggregate.Task FindByIdAndListTask(int taskId, int listTaskId)
        {
            return (from t in DbSet 
                    where t.Id == taskId && t.ListTaskId == listTaskId
                    select t).FirstOrDefault();
        }

        public void CreateTask(Domain.AggregateModels.TaskAggregate.Task task)
        {
            AddAsync(task);
        }

        public void DeleteTask(Domain.AggregateModels.TaskAggregate.Task task)
        {
            DbSet.Remove(task);
        }
    }
}
