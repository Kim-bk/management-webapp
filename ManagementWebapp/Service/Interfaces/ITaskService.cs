using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;


namespace Service.Interfaces
{
    public interface ITaskService
    {
        Task<TaskManagerResponse> GetTask(int taskId);
        Task<UserManagerResponse> AddLabelToTask(LabelRequest model);
        Task<UserManagerResponse> RemoveLabelInTask(LabelRequest model);
        Task<TaskManagerResponse> AddToDoToTask(ToDoRequest model);
        Task<TaskManagerResponse> ManageToDoItems(ToDoRequest model);
        Task<TaskManagerResponse> AssignMember(int taskId, string userId);
    }
}
