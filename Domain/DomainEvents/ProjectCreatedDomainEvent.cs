using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;
using MediatR;

namespace Domain.DomainEvents
{
    public class ProjectCreatedDomainEvent : INotification
    {
        public ProjectCreatedDomainEvent(Project project)
        {
            Project = project;
        }
        public Project Project { get; private set; }
    }
}
