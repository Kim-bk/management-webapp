using System.Threading.Tasks;
using Domain.AggregateModels.TaskAggregate;

namespace Domain.Interfaces.Repositories
{
    public interface IToDoRepository : IRepository<Todo>
    {
        Todo FindToDoItems(int taskId, int todoId);
    }
}
