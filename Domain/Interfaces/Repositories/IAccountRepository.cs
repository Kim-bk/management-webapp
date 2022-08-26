using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.AggregateModels.ProjectAggregate;

namespace Domain.Accounts
{
    public interface IAccountRepository
    {
        Task<List<Project>> GetUserProjects(string userId);
    }
}
