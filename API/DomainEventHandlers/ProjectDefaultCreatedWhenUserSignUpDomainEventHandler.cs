using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.DomainEvents;
using Domain.Interfaces.Repositories;
using MediatR;

namespace API.DomainEventHandlers
{
    public class ProjectDefaultCreatedWhenUserSignUpDomainEventHandler : INotificationHandler<ProjectDefaultCreatedWhenUserSignUpDomainEvent>
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectDefaultCreatedWhenUserSignUpDomainEventHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task Handle(ProjectDefaultCreatedWhenUserSignUpDomainEvent notification, CancellationToken cancellationToken)
        {
            // 1. Add user to Project Default
            notification.Project.Users.Add(notification.User);
        }
    }
}
