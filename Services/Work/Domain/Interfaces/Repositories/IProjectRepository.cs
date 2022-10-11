using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.AggregateModels.ProjectAggregate;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
    }
}
