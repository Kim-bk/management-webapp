using Domain.AggregateModels.ProjectAggregate;
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
