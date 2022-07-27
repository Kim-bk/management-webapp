using System;
using System.Collections.Generic;
using System.Linq;
using API.Context;
using Domain.Accounts;
using Domain.Entities;


namespace Infrastructure.Repositories
{
    public class AccountRepository : Repository<Project>, IAccountRepository
    {
        private AppDbContext _context;

        public AccountRepository(AppDbContext database, DbFactory dbFactory) : base(dbFactory)
        {
            _context = database;
        }
        
        public IList<Project> GetUserProjects(string userId)
        {
            var listProject = new List<Project>();

            var findUserProject = (from p in (_context.Projects).Cast<Project>()
                                join pm in (_context.ProjectMembers).Cast<ProjectMember>() on p.Id equals pm.ProjectId
                                where pm.UserId == userId
                                select new 
                                { 
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

        public Project CreateUserProject(string projectName)
        {
            try
            {
                var newProject = new Project
                {
                    Name = projectName
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

        public bool AddUserToProject(Project project, string userId)
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
    }
}
