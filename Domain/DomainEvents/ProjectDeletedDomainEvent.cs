using Domain.AggregateModels.ProjectAggregate;
using MediatR;

namespace Domain.DomainEvents
{
    public class ProjectDeletedDomainEvent : INotification
    {
        public ProjectDeletedDomainEvent(int projectId)
        {
            ProjectId = projectId;
        }
        public int ProjectId { get; private set; }
    }
}
