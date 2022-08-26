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
        private readonly IUnitOfWork _unitOfWork;
        public ListTaskDeletedDomainEventHandler(IListTaskRepository listTaskRepository, IUnitOfWork unitOfWork)
        {
            _listTaskRepository = listTaskRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ListTaskDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Find list task to handle
                var listTask = await _listTaskRepository.FindListTaskByIdAsync(notification.ListTaskId);

                // 2. Delete all tasks in it
                foreach (var task in listTask.Tasks)
                {
                    task.DeleteTask();
                }

                // 3. Delete list task
                _listTaskRepository.DelteListTask(listTask);
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw e;
            }
        }
    }
}
