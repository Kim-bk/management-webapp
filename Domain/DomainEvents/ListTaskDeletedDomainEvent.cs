using MediatR;

namespace Domain.DomainEvents
{
    public class ListTaskDeletedDomainEvent : INotification
    {
        public ListTaskDeletedDomainEvent(int listTaskId)
        {
            ListTaskId = listTaskId;
        }
        public int ListTaskId { get; private set; }
    }
}
