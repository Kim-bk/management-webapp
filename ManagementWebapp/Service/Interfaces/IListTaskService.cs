using System.Threading.Tasks;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;

namespace Service.Interfaces
{
    public interface IListTaskService
    {
        Task<UserManagerResponse> AddTaskToList(CommonRequest model, string userId);
        Task<ListTaskManagerResponse> GetAllTasks(int listTaskId);
        Task<UserManagerResponse> MoveTask(TaskRequest model);
    }
}
