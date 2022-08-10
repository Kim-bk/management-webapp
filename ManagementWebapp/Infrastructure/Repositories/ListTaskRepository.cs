using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ListTaskRepository : Repository<ListTask>, IListTaskRepository
    {
        private readonly IMapper _mapper;
        public ListTaskRepository(DbFactory dbFactory, IMapper mapper) : base(dbFactory)
        {
            _mapper = mapper;
        }
        public async void Create(ListTask listTask)
        {
            await DbSet.AddAsync(listTask);
        }

        public async Task<ListTask> FindByNameAsync(string nameListTask)
        {
            return await DbSet.Where(lt => lt.Title == nameListTask).FirstOrDefaultAsync();
        }

        public async Task<ListTask> FindListTaskByIdAsync(int listTaskId)
        {
            return await DbSet.FindAsync(listTaskId);
        }
        public async Task<List<TaskDTO>> GetTasksInList(int listTaskId)
        {
            var listTask = await FindListTaskByIdAsync(listTaskId);
            if (listTask == null)
            {
                return null;
            }

            return _mapper.MapTasks(listTask);
        }
    }
}
