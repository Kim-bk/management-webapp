using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using System.Linq;
using Domain.DTOs;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class TaskRepository : Repository<Domain.Entities.Task>, ITaskRepository
    {
        private readonly IMappering _mapper;
        public TaskRepository(DbFactory dbFactory, IMappering mapper) : base(dbFactory)
        {
            _mapper = mapper;
        }
        public async Task<TaskDTO> GetByIdAsync(int taskId)
        {
            var task = await FindByIdAsync(taskId);
            return new TaskDTO
            {
                TaskId = task.Id,
                Title = task.Title,
                Labels = _mapper.MapLabels(task),
                Members = _mapper.MapMembers(task),
                Todos = _mapper.MapTodos(task)
            };
        }

        public async Task<Domain.Entities.Task> FindByIdAsync(int taskId)
        {
            return await DbSet.FindAsync(taskId);
        }

        public Domain.Entities.Task FindByIdAndListTask(int taskId, int listTaskId)
        {
            return (from t in DbSet 
                    where t.Id == taskId && t.ListTaskId == listTaskId
                    select t).FirstOrDefault();
        }
    }
}
