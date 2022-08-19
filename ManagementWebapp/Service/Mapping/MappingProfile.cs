using System.Collections.Generic;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Project, ProjectDTO>()
                .ForMember(des => des.ListTasks, act => act.Ignore());
        }
    }
}
