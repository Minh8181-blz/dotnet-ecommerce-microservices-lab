using BB.EventBus.Events;
using BB.EventBus.RabbitMQ.Enums;
using System;

namespace BB.EventBus.RabbitMQ.Models
{
    public class ConsumerSpecification
    {
        public static ConsumerSpecification CreateSpecification<T>(ConsumerTypeEnum type, string queue, string exchange, string routingKey,
            MessageContentTypeEnum contentType = MessageContentTypeEnum.Json, bool multipleAck = false, ushort? prefetchCount = null)
            where T : IntegrationEvent
        {
            return new ConsumerSpecification(typeof(T), type, queue, exchange, routingKey, contentType, multipleAck, prefetchCount);
        }

        private ConsumerSpecification(Type eventType, ConsumerTypeEnum type, string queue, string exchange, string routingKey,
            MessageContentTypeEnum contentType = MessageContentTypeEnum.Json, bool multipleAck = false, ushort? prefetchCount = null)
        {
            EventType = eventType;
            Type = type;
            Queue = queue;
            Exchange = exchange;
            RoutingKey = routingKey;
            ContentType = contentType;
            MultipleAck = multipleAck;
            PrefetchCount = prefetchCount;
        }

        public Type EventType { get; } 
        public ConsumerTypeEnum Type { get; }
        public string Queue { get; }
        public string Exchange { get; }
        public string RoutingKey { get; }
        public MessageContentTypeEnum ContentType { get; }
        public bool MultipleAck { get; }
        public ushort? PrefetchCount { get; }
    }
}
