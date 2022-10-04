using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.AggregateModels.UserAggregate;
using Domain.Interfaces;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace API.IntegrationEvents.EventHandlers
{
    public class UserUpdatedIntegrationEventHandler : IIntegrationEventHandler<UserUpdatedIntegrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserUpdatedIntegrationEventHandler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserUpdatedIntegrationEventHandler(ILogger<UserUpdatedIntegrationEventHandler> logger, IUnitOfWork unitOfWork
              , UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task Handle(UserUpdatedIntegrationEvent @event)
        {
            try
            {
                // 1. Find user
                var user = await _userManager.FindByIdAsync(@event.UserId);

                // 2. Check
                if (user == null)
                {
                    throw new ArgumentNullException("Can't find user !");
                }

                user.Email = @event.Email;
                user.PhoneNumber = @event.PhoneNumber;

                await _userManager.UpdateAsync(user);
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError(ex.Message);
            }
        }
    }
}
