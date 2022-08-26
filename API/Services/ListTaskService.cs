using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Service.Interfaces;
using System.Linq;
using API.DTOs.Responses;
using API.Services.Interfaces;
using API.Services;

namespace Service
{
    public class ListTaskService : BaseService, IListTaskService
    {
        private readonly IListTaskRepository _listTaskRepository;

        public ListTaskService(IListTaskRepository listTaskRepository, IUnitOfWork unitOfWork,
                           IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _listTaskRepository = listTaskRepository;
        }
       
        public async Task<ListTaskManagerResponse> GetAllTasks(int listTaskId)
        {
            // 1. Validate
            if (listTaskId == 0)
            {
                return new ListTaskManagerResponse
                {
                    Message = "List Task Id is null",
                    IsSuccess = true,
                };
            }

            var listTask = await _listTaskRepository.FindListTaskByIdAsync(listTaskId);
            if (listTask == null)
            {
                return new ListTaskManagerResponse
                {
                    Message = "List Task Id is not found!",
                    IsSuccess = true,
                };
            }

            // 2. Get all tasks in list task
            var tasks = await _listTaskRepository.GetTasksInList(listTaskId);

            // 3. Return result
            return new ListTaskManagerResponse
            {
                Message = "Get all tasks in list success!",
                IsSuccess = true,
                Task = _mapper.MapTasks(tasks).OrderBy(t => t.Position).ToList()
            };
        }
    }
}
