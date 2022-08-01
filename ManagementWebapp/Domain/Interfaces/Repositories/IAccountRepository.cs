using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Accounts
{
    public interface IAccountRepository
    {
        IQueryable<ICollection<Project>> GetUserProjects(string userId);
    }
}
