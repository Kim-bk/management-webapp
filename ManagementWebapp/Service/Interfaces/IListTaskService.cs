using System.Threading.Tasks;
using Service.DTOs.Responses;
using Service.DTOS.Responses;

namespace Service.Interfaces
{
    public interface IListTaskService
    {
        Task<UserManagerResponse> AddTaskToList(string titleTask, int listTaskId);
        Task<ListTaskManagerResponse> GetAllTasks(int listTaskId);
    }
}
