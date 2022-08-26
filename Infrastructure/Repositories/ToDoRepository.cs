using System.Linq;
using System.Threading.Tasks;
using Domain.AggregateModels.TaskAggregate;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ToDoRepository : Repository<Todo>, IToDoRepository
    {
        public ToDoRepository(DbFactory dbFactory) : base(dbFactory)
        { }

        public Todo FindToDoItems(int taskId, int todoId)
        {
            return (from todo in DbSet
                    where todo.Id == todoId && todo.TaskId == taskId
                    select todo).FirstOrDefault();
        }

        public async Task<Todo> FindByNameAsync(string title)
        {
            return await DbSet.Where(td => td.Title == title).FirstOrDefaultAsync();
        }
    }
}
