using System.Collections.Generic;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMapper
    {
        List<LabelDTO> MapLabels(Domain.Entities.Task task);
        List<MemberDTO> MapMembers(Domain.Entities.Task task);
        List<TodoDTO> MapTodos(Domain.Entities.Task task);
        List<TaskDTO> MapTasks(ListTask listTask);
        List<ListTaskDTO> MapListTasks(Project project);

    }
}
