using BB.EventBus.Events;
using BB.EventBus.RabbitMQ.Enums;
using System;

namespace BB.EventBus.RabbitMQ.Models
{
    public class PublisherSpecification
    {
        public static PublisherSpecification CreateSpecification<T>(PublisherTypeEnum type, string exchange,
            string routingKey, MessageContentTypeEnum contentType = MessageContentTypeEnum.Json)
            where T : IntegrationEvent
        {
            return new PublisherSpecification(typeof(T), type, exchange, routingKey, contentType);
        }

        private PublisherSpecification(Type eventType, PublisherTypeEnum type, string exchange, string routingKey,
            MessageContentTypeEnum contentType = MessageContentTypeEnum.Json)
        {
            EventType = eventType;
            Type = type;
            Exchange = exchange;
            RoutingKey = routingKey;
            ContentType = contentType;
        }

        public Type EventType { get; }
        public PublisherTypeEnum Type { get; }
        public string Exchange { get; }
        public string RoutingKey { get; }
        public MessageContentTypeEnum ContentType { get; }
    }
}
