using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;

namespace Domain.Accounts
{
    public interface IAccountRepository : IRepository<ApplicationUser>
    {
    }
}
