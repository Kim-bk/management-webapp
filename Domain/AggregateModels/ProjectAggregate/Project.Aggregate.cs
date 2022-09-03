using System;
using System.Diagnostics.CodeAnalysis;
using Domain.AggregateModels.UserAggregate;
using Domain.Base;
using System.Linq;
using Domain.SeedWork;
using Domain.DomainEvents;

namespace Domain.AggregateModels.ProjectAggregate
{
    public partial class Project : EntityBase, IAggregateRoot
    {
        public Project(string name) : this()
        {
            Update(name);
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
            ListTasks.Add(new ListTask
            {
                Title = name
            });   
        }
        public void RemoveListTask(int listTaskId)
        {
            ListTasks.Remove(ListTasks.FirstOrDefault(lt => lt.ListTaskId == listTaskId));
        }
        public void AddMember(ApplicationUser user)
        {
            Users.Add(user);
        }
        public void AddProjectDefault(string userId)
        {
            AddDomainEvent(new ProjectDefaultCreatedWhenUserSignUpDomainEvent(this, userId));
        }
        public void DeleteListTask(int listTaskId)
        {
            AddDomainEvent(new ListTaskDeletedDomainEvent(listTaskId));
        }
        public void DeleteProject()
        {
            AddDomainEvent(new ProjectDeletedDomainEvent(Id));
        }
    }
}
