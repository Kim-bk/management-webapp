using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.DomainEvents;
using Domain.Interfaces.Repositories;
using MediatR;

namespace API.DomainEventHandlers
{
    public class ProjectDeletedDomainEventHandler : INotificationHandler<ProjectDeletedDomainEvent>
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectDeletedDomainEventHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task Handle(ProjectDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            // 1. Find the project
            var project = await _projectRepository.FindByIdAsync(notification.ProjectId);

            // 2. Delete all its list task
            foreach (var listTask in project.ListTasks)
            {
                project.DeleteListTask(listTask.ListTaskId);
            }
            // 4. Delete the project
            _projectRepository.DeleteProject(project);
        }
    }
}
