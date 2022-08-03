using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IToDoRepository
    {
        Todo FindToDoItems(int taskId, int todoId);
    }
}
