using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.AggregateModels.UserAggregate;
using Domain.DomainEvents;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.DomainEventHandlers
{
    public class ProjectDefaultCreatedWhenUserSignUpDomainEventHandler : INotificationHandler<ProjectDefaultCreatedWhenUserSignUpDomainEvent>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProjectDefaultCreatedWhenUserSignUpDomainEventHandler(IProjectRepository projectRepository
            , UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
        }
        public async Task Handle(ProjectDefaultCreatedWhenUserSignUpDomainEvent notification, CancellationToken cancellationToken)
        {
            // 1. Find user
            var user = await _userManager.FindByIdAsync(notification.UserId);
           
            // 2. Add user to Project Default
            notification.Project.Users.Add(user);
        }
    }
}
