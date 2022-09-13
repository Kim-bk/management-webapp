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
            AddDomainEvent(new ProjectCreatedDomainEvent(this));
        }

        public void Update(string name)
        {
            Name = name;
        }

        public void CreateListTask(string nameListTask)
        {
            // Check if list task already exists
            if (ListTasks.FirstOrDefault(lt => lt.Title == nameListTask) == null)
            {
                ListTasks.Add(new ListTask
                {
                    Title = nameListTask
                });
            }
        }
        public void RemoveListTask(int listTaskId)
        {
            ListTasks.Remove(ListTasks.FirstOrDefault(lt => lt.ListTaskId == listTaskId));
        }
        public void AddMember(ApplicationUser user)
        {
            Users.Add(user);
        }
        public void DeleteListTask(int listTaskId)
        {
            AddDomainEvent(new ListTaskDeletedDomainEvent(listTaskId));
        }
        public void DeleteProject()
        {
            foreach(var listTask in ListTasks)
            {
                DeleteListTask(listTask.ListTaskId);
            }
        }
    }
}
