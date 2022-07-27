using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Context;
using API.Entities;
using Domain.Accounts;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class AccountRepository : Repository<Project>, IAccountRepository
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _userSM;
        private AppDbContext _context;

        public AccountRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> userSM,
            AppDbContext database, DbFactory dbFactory) : base(dbFactory)
        {
            _userManager = userManager;
            _userSM = userSM;
            _context = database;
        }
        
        public Task<IdentityResult> RegisterUser(RegisterRequest model)
        {
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.UserName,
            };

            return _userManager.CreateAsync(identityUser, model.Password);
        }

        public Task<bool> CheckPassword(IdentityUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public Task<IdentityUser> FindUserByName(string name)
        {
            return _userManager.FindByNameAsync(name);
        }

        public IList<Project> GetUserProjects(string userId)
        {
            var listProject = new List<Project>();

            var findUserProject = (from p in (_context.Projects).Cast<Project>()
                                join pm in (_context.ProjectMembers).Cast<ProjectMember>() on p.Id equals pm.ProjectId
                                where pm.UserId == userId
                                select new { 
                                p.Id,
                                p.Name
                                }).ToList();

            foreach(var item in findUserProject)
            {
                var p = new Project
                {
                    Id = item.Id,
                    Name = item.Name
                };

                listProject.Add(p);
            }

            return listProject;
        }

        public Project CreateUserProject(ProjectRequest model)
        {
            try
            {
                var newProject = new Project
                {
                    Name = model.Name
                };

                _context.Projects.Add(newProject);
                _context.SaveChanges();
                return newProject;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AssignUserToProject(Project project, string userId)
        {
            try
            {
                var newProjectMember = new ProjectMember
                {
                    Project = project,
                    UserId = userId,
                };

                _context.ProjectMembers.Add(newProjectMember);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UserLogout()
        {
            try
            {
                await _userSM.SignOutAsync();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
