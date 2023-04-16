using BB.EventBus.Events;

namespace BB.EventBus.Interfaces
{
    public interface IIntegrationEventBus
    {
        bool PublishEvent(IntegrationEvent @event);
        bool SubscribeEvent<T>() where T : IntegrationEvent;
    }
}
