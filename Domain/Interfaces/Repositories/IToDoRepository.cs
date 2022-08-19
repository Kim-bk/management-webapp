using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IToDoRepository
    {
        Todo FindToDoItems(int taskId, int todoId);
        Task<Todo> FindByNameAsync(string title);
    }
}
