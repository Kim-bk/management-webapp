﻿using AutoMapper;
using API.DTOs;
using Domain.Entities;

namespace API.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Project -> ProjectDTO
            CreateMap<Project, ProjectDTO>()
                .ForMember(des => des.ListTasks, act => act.Ignore());

            // ListTask -> ListTaskDTO
            CreateMap<ListTask, ListTaskDTO>()
                .ForMember(des => des.Tasks, act => act.Ignore());

            // Task -> TaskDTO
            CreateMap<Task, TaskDTO>()
                .ForMember(s => s.TaskId, o => o.MapFrom(t => t.Id))
                .ForMember(des => des.Todos, act => act.Ignore())
                .ForMember(des => des.Labels, act => act.Ignore())
                .ForMember(des => des.Members, act => act.Ignore());

            // Label -> LabelDTO
            CreateMap<Label, LabelDTO>()
                .ForMember(des => des.LabelId, o => o.MapFrom(l => l.Id));

            // Todo -> TodoDTO
            CreateMap<Todo, TodoDTO>()
                .ForMember(des => des.TodoId, o => o.MapFrom(t => t.Id));

            // ApplicationUser -> MemberDTO
            CreateMap<ApplicationUser, MemberDTO>()
                .ForMember(des => des.UserId, o => o.MapFrom(u => u.Id));

        }
    }
}
