using System.Threading;
using System.Threading.Tasks;
using Domain.DomainEvents;
using MediatR;

namespace API.DomainEventHandlers
{
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjectCreatedDomainEvent>
    {
        public ProjectCreatedDomainEventHandler()
        {
        }
        public Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // 1. Get project
            var project = notification.Project;

            // 2. Create List task default
            project.CreateListTask("Default ListTask");
            return Task.CompletedTask;
        }
    }
}
