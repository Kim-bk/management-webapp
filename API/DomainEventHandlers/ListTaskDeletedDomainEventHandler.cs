using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.DomainEvents;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace API.DomainEventHandlers
{
    public class ListTaskDeletedDomainEventHandler : INotificationHandler<ListTaskDeletedDomainEvent>
    {
        private readonly IListTaskRepository _listTaskRepository;
        public ListTaskDeletedDomainEventHandler(IListTaskRepository listTaskRepository)
        {
            _listTaskRepository = listTaskRepository;
        }
        public async Task Handle(ListTaskDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            // 1. Find list task to handle
            var listTask = await _listTaskRepository.FindAsync(lt => lt.ListTaskId == notification.ListTaskId);

            // 2. Delete all components (todo, member, label)
            // of each tasks in the list
            foreach (var task in listTask.Tasks)
            {
                task.DeleteTask();
            }

            // 3. Delete list task
            _listTaskRepository.Delete(listTask);
        }
    }
}
