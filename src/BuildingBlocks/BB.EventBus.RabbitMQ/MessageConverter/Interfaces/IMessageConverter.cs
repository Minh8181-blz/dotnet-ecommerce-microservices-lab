using BB.EventBus.Events;
using System;

namespace BB.EventBus.RabbitMQ.MessageConverter.Interfaces
{
    public interface IMessageConverter
    {
        byte[] Serialize(IntegrationEvent message);
        T Deserialize<T>(ReadOnlyMemory<byte> body) where T : IntegrationEvent;
    }
}
