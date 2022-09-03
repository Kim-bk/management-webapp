using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;
using MediatR;

namespace Domain.DomainEvents
{
    public class ProjectDefaultCreatedWhenUserSignUpDomainEvent : INotification
    {
        public ProjectDefaultCreatedWhenUserSignUpDomainEvent(Project project, string userId)
        {
            UserId = userId;
            Project = project;
        }
        public string UserId { get; private set; }
        public Project Project { get; private set; }
    }
}
