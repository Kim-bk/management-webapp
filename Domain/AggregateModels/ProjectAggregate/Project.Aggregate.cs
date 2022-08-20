using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public partial class Project
    {
        public Project(string name) : this()
        {
            this.Update(name);
        }

        public void Update([NotNull] string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Name can not be null or white space");
            }
            Name = name;
        }

        public void CreateListTask(string name)
        {
            this.ListTasks.Add(new ListTask
            {
                Title = name
            });
        }

        public void AddMember(ApplicationUser user)
        {
            this.Users.Add(user);
        }
    }
}
