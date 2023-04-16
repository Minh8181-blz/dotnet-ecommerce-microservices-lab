using BB.EventBus.RabbitMQ.Enums;
using BB.EventBus.RabbitMQ.MessageConverter;
using BB.EventBus.RabbitMQ.Models;
using RabbitMQ.Client;

namespace BB.EventBus.RabbitMQ.Consumers
{
    class ConsumerFactory
    {
        private readonly IConnection _connection;
        private readonly MessageConverterFactory _messageConverterFactory;

        public ConsumerFactory(
            IConnection connection,
            MessageConverterFactory messageConverterFactory)
        {
            _connection = connection;
            _messageConverterFactory = messageConverterFactory;
        }

        public Consumer Create(ConsumerSpecification specification)
        {
            if (specification.Type == ConsumerTypeEnum.Basic)
            {
                var deserializer = _messageConverterFactory.CreateByMessageContentType(specification.ContentType);
                return new BasicConsumer(_connection, specification.Queue, deserializer, specification.PrefetchCount, specification.MultipleAck);
            }
            else
            {
                var deserializer = _messageConverterFactory.CreateByMessageContentType(specification.ContentType);
                return new BasicConsumer(_connection, specification.Queue, deserializer);
            }
        }
    }
}
