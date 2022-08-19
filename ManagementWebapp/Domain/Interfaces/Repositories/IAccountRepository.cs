using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Accounts
{
    public interface IAccountRepository
    {
        Task<List<Project>> GetUserProjects(string userId);
    }
}
