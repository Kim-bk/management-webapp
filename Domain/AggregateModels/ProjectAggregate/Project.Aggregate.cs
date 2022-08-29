using System;
using System.Diagnostics.CodeAnalysis;
using Domain.AggregateModels.UserAggregate;
using Domain.Base;
using System.Linq;
using Domain.SeedWork;
using Domain.DomainEvents;

namespace Domain.AggregateModels.ProjectAggregate
{
    public partial class Project : Entity, IAggregateRoot
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
        public void AddMember(ApplicationUser user)
        {
            AddDomainEvent(new ProjectDefaultCreatedWhenUserSignUpDomainEvent(this, user));
        }
        public void RemoveListTask(int listTaskId)
        {
            ListTasks.Remove(ListTasks.FirstOrDefault(lt => lt.ListTaskId == listTaskId));
        }

        public void DeleteListTask(int listTaskId)
        {
            AddDomainEvent(new ListTaskDeletedDomainEvent(listTaskId));
        }
    }
}
