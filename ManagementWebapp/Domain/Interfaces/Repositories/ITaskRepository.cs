using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<Domain.Entities.Task> FindByIdAsync(int taskId);
        Domain.Entities.Task FindByTwoId(int taskId, int listTaskId);
    }
}
