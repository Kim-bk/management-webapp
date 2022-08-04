using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Accounts
{
    public interface IAccountRepository
    {
        List<ProjectDTO> GetUserProjects(string userId);
    }
}
