using System.Collections.Generic;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMapperCustom
    {
        List<LabelDTO> MapLabels(List<Label> labels);
        List<MemberDTO> MapMembers(List<ApplicationUser> members);
        List<TodoDTO> MapTodos(List<Todo> todos);
        List<TaskDTO> MapTasks(List<Task> tasks);
        List<ListTaskDTO> MapListTasks(List<ListTask> listTasks);
        
    }
}
