using System.Collections.Generic;
using MediatR;
using Domain.Shared;

namespace Domain.Base
{
    public interface IEntityBase
    {
        public void AddDomainEvent(INotification eventItem)
        {
            Common._domainEvents = Common._domainEvents ?? new List<INotification>();
            Common._domainEvents.Add(eventItem);
        }
    }
}
