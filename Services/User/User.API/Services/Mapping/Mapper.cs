using System.Collections.Generic;
using API.DTOs;
using API.Services.Interfaces;
using System.Linq;
using AutoMapper;
using Domain.AggregateModels.UserAggregate;

namespace Service
{
    public class Mapper : IMapperCustom
    {
        private readonly IMapper _autoMapper;
        public Mapper(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }
     
        public List<MemberDTO> MapMembers(List<ApplicationUser> members)
        {
            return _autoMapper.Map<List<ApplicationUser>, List<MemberDTO>>(members);
        }
    }
}
