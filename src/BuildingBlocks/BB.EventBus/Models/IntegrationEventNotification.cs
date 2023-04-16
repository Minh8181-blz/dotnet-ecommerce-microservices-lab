using BB.EventBus.Events;
using MediatR;

namespace BB.EventBus.Models
{
    public class IntegrationEventNotification<T> : IRequest<bool> where T : IntegrationEvent
    {
        public IntegrationEventNotification(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
