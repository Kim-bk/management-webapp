using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;
using MediatR;

namespace Domain.DomainEvents
{
    public class ProjectDefaultCreatedWhenUserSignUpDomainEvent : INotification
    {
        public ProjectDefaultCreatedWhenUserSignUpDomainEvent(Project project, ApplicationUser user)
        {
            User = user;
            Project = project;
        }
        public ApplicationUser User { get; set; }
        public Project Project { get; set; }
    }
}
