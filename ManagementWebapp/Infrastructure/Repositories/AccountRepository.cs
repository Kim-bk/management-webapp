using System.Collections.Generic;
using System.Linq;
using Domain.Accounts;
using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        public AccountRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
        public List<ProjectDTO> GetUserProjects(string userId)
        {
            // 1. Find all projects of user
            var getCollectionProjects = DbSet.Where(u => u.Id == userId).Select(p => p.Projects);
           
            // 2. Init list project to store DTO and return to client
            var storeProjects = new List<ProjectDTO>();
            foreach (var listProject in getCollectionProjects)
            {
                foreach (var project in listProject)
                {
                    var p = new ProjectDTO
                    {
                        ProjectId = project.Id,
                        Name = project.Name,
                    };
                    storeProjects.Add(p);
                }
            }
            return storeProjects;
        }
    }
}
