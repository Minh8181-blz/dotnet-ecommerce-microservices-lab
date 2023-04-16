using BB.EventBus.Events;
using BB.EventBus.RabbitMQ.MessageConverter.Interfaces;
using RabbitMQ.Client;
using System;

namespace BB.EventBus.RabbitMQ.Publishers
{
    abstract class Publisher
    {
        protected readonly IConnection _connection;
        protected IModel _channel;
        protected readonly IMessageConverter _serializer;
        protected readonly string _exchange;
        protected readonly string _routingKey;
        protected bool _exchangeDeclared;
        public Type EventType { get; }

        public Publisher(
            IConnection connection,
            string exchange,
            string routingKey,
            IMessageConverter serializer,
            Type eventType)
        {
            _connection = connection;
            _exchange = exchange;
            _routingKey = routingKey;
            _serializer = serializer;
            EventType = eventType;
            _exchangeDeclared = false;
        }

        public bool Publish(IntegrationEvent @event)
        {
            if (_channel == null)
            {
                _channel = _connection.CreateModel();
            }

            if (!_exchangeDeclared)
            {
                _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Topic, durable: true);
                _exchangeDeclared = true;
            }

            return DoPublish(@event);
        }

        protected abstract bool DoPublish(IntegrationEvent @event);
    }
}
