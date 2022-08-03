using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Service.DTOs.Requests;
using Service.DTOs.Responses;
using Service.DTOS.Responses;

namespace Service.Interfaces
{
    public interface ITaskService
    {
        Task<TaskManagerResponse> GetAllComponentsOfTask(int taskId);
        Task<UserManagerResponse> AddLabel(LabelRequest model);
        Task<UserManagerResponse> RemoveLabel(LabelRequest model);
        Task<TaskManagerResponse> AddToDo(ToDoRequest model);
        Task<TaskManagerResponse> ManageToDoItems(ToDoRequest model);
        Task<TaskManagerResponse> AssignMember(int taskId, string userId);
    }
}
