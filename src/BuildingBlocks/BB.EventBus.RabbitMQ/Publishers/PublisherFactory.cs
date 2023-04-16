using BB.EventBus.RabbitMQ.Enums;
using BB.EventBus.RabbitMQ.MessageConverter;
using BB.EventBus.RabbitMQ.Models;
using RabbitMQ.Client;

namespace BB.EventBus.RabbitMQ.Publishers
{
    class PublisherFactory
    {
        private readonly IConnection _connection;
        private readonly MessageConverterFactory _messageConverterFactory;

        public PublisherFactory(
            IConnection connection,
            MessageConverterFactory messageConverterFactory)
        {
            _connection = connection;
            _messageConverterFactory = messageConverterFactory;
        }

        public Publisher Create(PublisherSpecification specification)
        {
            if (specification.Type == PublisherTypeEnum.Basic)
            {
                var deserializer = _messageConverterFactory.CreateByMessageContentType(specification.ContentType);
                return new BasicPublisher(_connection, specification.Exchange, specification.RoutingKey, deserializer, specification.EventType);
            }
            else
            {
                var deserializer = _messageConverterFactory.CreateByMessageContentType(specification.ContentType);
                return new BasicPublisher(_connection, specification.Exchange, specification.RoutingKey, deserializer, specification.EventType);
            }
        }
    }
}
