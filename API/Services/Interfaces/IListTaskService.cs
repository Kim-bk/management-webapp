using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;

namespace Service.Interfaces
{
    public interface IListTaskService
    {
        Task<ListTaskManagerResponse> GetAllTasks(int listTaskId);
    }
}
