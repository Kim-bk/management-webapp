using System.Collections.Generic;
using API.DTOs;
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.TaskAggregate;
using Domain.AggregateModels.UserAggregate;

namespace API.Services.Interfaces
{
    public interface IMapperCustom
    {
        List<LabelDTO> MapLabels(List<Label> labels);
        List<MemberDTO> MapMembers(List<ApplicationUser> members);
        List<TodoDTO> MapTodos(List<Todo> todos);
        List<TaskDTO> MapTasks(List<Task> tasks);
        List<ListTaskDTO> MapListTasks(List<ListTask> lisTasks);
    }
}
