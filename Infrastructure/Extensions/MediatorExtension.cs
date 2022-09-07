using System.Linq;
using System.Threading.Tasks;
using Domain.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
        {
           var domainEntities = ctx.ChangeTracker
                  .Entries<EntityBase>()
                  .Where(x => x.Entity != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
