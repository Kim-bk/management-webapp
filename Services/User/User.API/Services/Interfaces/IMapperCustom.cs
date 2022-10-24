using System.Collections.Generic;
using API.DTOs;
using Domain.AggregateModels.UserAggregate;

namespace API.Services.Interfaces
{
    public interface IMapperCustom
    {
        List<MemberDTO> MapMembers(List<ApplicationUser> members);
    }
}
