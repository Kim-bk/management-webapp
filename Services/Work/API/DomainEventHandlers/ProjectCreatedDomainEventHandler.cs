using System.Threading;
using System.Threading.Tasks;
using Domain.AggregateModels.ProjectAggregate;
using Domain.DomainEvents;
using Domain.Interfaces.Repositories;
using MediatR;

namespace API.DomainEventHandlers
{
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjectCreatedDomainEvent>
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectCreatedDomainEventHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // 1. Get project
            var project = notification.Project;

            // 2. Create List task default
             project.CreateListTask("Default ListTask");
           
        }
    }
}
