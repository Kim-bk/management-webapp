using System;

namespace EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        public class SubscriptionInfo
        {
            public Type HandlerType { get; }

            private SubscriptionInfo(bool isDynamic, Type handlerType)
            {
                HandlerType = handlerType;
            }

            public static SubscriptionInfo Typed(Type handlerType) =>
                new SubscriptionInfo(false, handlerType);
        }
    }
}
