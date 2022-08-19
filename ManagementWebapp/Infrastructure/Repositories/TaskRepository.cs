﻿using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using System.Linq;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskRepository : Repository<Domain.Entities.Task>, ITaskRepository
    {
        private readonly IMapperCustom _mapper;
        public TaskRepository(DbFactory dbFactory, IMapperCustom mapper) : base(dbFactory)
        {
            _mapper = mapper;
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
