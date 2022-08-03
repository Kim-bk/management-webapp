using System.Threading.Tasks;
using Service.DTOs.Requests;
using Service.DTOs.Responses;
using Service.DTOS.Responses;

namespace Service.Interfaces
{
    public interface IListTaskService
    {
        Task<UserManagerResponse> AddTaskToList(CommonRequest model, string userId);
        Task<ListTaskManagerResponse> GetAllTasks(int listTaskId);
        Task<UserManagerResponse> MoveTask(TaskRequest model);
    }
}
