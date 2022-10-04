using EventBus.Events;

namespace API.IntegrationEvents
{
    public class UserUpdatedIntegrationEvent : IntegrationEvent
    { 
        public UserUpdatedIntegrationEvent(string userId, string email, string phoneNumber)
        {
            UserId = userId;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        public string UserId { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; set; }
    }
}
