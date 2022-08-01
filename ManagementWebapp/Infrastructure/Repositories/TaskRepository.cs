using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TaskRepository : Repository<Domain.Entities.Task>, ITaskRepository
    {
        public TaskRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
        public async Task<Domain.Entities.Task> FindByIdAsync(int taskId)
        {
            return await DbSet.FindAsync(taskId);
        }
        public Domain.Entities.Task FindByTwoId(int taskId, int listTaskId)
        {
            return (from t in DbSet 
                    where t.Id == taskId && t.ListTaskId == listTaskId
                    select t).FirstOrDefault();
        }
    }
}
