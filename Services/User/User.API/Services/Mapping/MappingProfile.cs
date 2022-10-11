using AutoMapper;
using API.DTOs;
using Domain.AggregateModels.UserAggregate;

namespace API.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        { 
            // ApplicationUser -> MemberDTO
            CreateMap<ApplicationUser, MemberDTO>()
                .ForMember(des => des.UserId, o => o.MapFrom(u => u.Id));
        }
    }
}
