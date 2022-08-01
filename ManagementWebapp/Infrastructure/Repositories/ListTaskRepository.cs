using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class ListTaskRepository : Repository<ListTask>, IListTaskRepository
    {
        public ListTaskRepository(DbFactory dbFactory) : base(dbFactory)
        { }
        public async void Create(ListTask listTask)
        {
            await DbSet.AddAsync(listTask);
        
        }
        public async Task<ListTask> FindListTaskByIdAsync(int listTaskId)
        {
            return await DbSet.FindAsync(listTaskId);
        }

        public IQueryable<ICollection<Domain.Entities.Task>> GetAllTasks(int listTaskId)
        {
            return DbSet.Where(lt => lt.ListTaskId == listTaskId).Select(t => t.Tasks);
        }
    }
}
