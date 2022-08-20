using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;

namespace Service.Interfaces
{
    public interface IListTaskService
    {
        Task<UserManagerResponse> AddTaskToList(int listTaskId, CommonRequest model, string userId);
        Task<ListTaskManagerResponse> GetAllTasks(int listTaskId);
    }
}
