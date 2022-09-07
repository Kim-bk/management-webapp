using API.Services.Interfaces;
using Domain.Interfaces;

namespace API.Services
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapperCustom _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapperCustom mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
