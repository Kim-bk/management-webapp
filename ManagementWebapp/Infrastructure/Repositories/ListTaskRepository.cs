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
        public ListTaskRepository(DbFactory dbFactory) : base(dbFactory)
        {
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
        public async Task<List<Domain.Entities.Task>> GetTasksInList(int listTaskId)
        {
            return await DbSet.Where(lt => lt.ListTaskId == listTaskId)
                .SelectMany(t => t.Tasks)
                .ToListAsync();
        }
    }
}
