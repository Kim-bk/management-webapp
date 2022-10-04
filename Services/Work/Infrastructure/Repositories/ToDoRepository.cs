using Domain.AggregateModels.TaskAggregate;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class ToDoRepository : Repository<Todo>, IToDoRepository
    {
        public ToDoRepository(DbFactory dbFactory) : base(dbFactory)
        { } 
    }
}
