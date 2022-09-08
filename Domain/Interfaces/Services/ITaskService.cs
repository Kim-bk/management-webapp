using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;
using Domain.AggregateModels.TaskAggregate;
using Domain.AggregateModels.UserAggregate;

namespace Service.Interfaces
{
    public interface ITaskService
    {
        Task<TaskManagerResponse> GetTask(int taskId);
        Task<UserManagerResponse> AddLabelToTask(LabelRequest request);
        Task<UserManagerResponse> RemoveLabelInTask(LabelRequest request);
        Task<TaskManagerResponse> AddToDoToTask(ToDoRequest request);
        Task<TaskManagerResponse> UpdateTodoItems(ToDoRequest request);
        Task<TaskManagerResponse> AssignMember(int taskId, string userId);
        Task<UserManagerResponse> MoveTask(int taskId, MoveTaskRequest request);
        Task<UserManagerResponse> CreateTask(TaskRequest request, string userId);
    }
}
