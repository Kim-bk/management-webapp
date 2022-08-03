using System.Linq;
using Domain.Entities;
using Domain.Interfaces.Repositories;

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
    }
}
