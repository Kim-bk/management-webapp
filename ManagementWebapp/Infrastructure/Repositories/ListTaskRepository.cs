using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class ListTaskRepository : Repository<ListTask>, IListTaskRepository
    {
        private IMapper _mapper;
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
            return await DbSet.FindAsync(nameListTask);
        }

        public async Task<ListTask> FindListTaskByIdAsync(int listTaskId)
        {
            return await DbSet.FindAsync(listTaskId);
        }

        public IQueryable<ICollection<Domain.Entities.Task>> GetAllTasks(int listTaskId)
        {
            return DbSet.Where(lt => lt.ListTaskId == listTaskId).Select(t => t.Tasks);
        }

        public List<TaskDTO> GetAllTasks(ListTask listTask)
        {
            return _mapper.MapTasks(listTask);
        }
    }
}
