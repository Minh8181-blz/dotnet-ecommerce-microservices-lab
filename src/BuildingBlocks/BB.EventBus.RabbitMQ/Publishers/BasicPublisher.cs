using BB.EventBus.Events;
using BB.EventBus.RabbitMQ.MessageConverter.Interfaces;
using RabbitMQ.Client;
using System;

namespace BB.EventBus.RabbitMQ.Publishers
{
    class BasicPublisher : Publisher
    {
        public BasicPublisher(
            IConnection connection,
            string exchange,
            string routingKey,
            IMessageConverter serializer,
            Type eventType)
            :base(connection, exchange, routingKey, serializer, eventType)
        {
        }

        protected override bool DoPublish(IntegrationEvent @event)
        {
            var body = _serializer.Serialize(@event);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: _exchange,
                                 routingKey: _routingKey,
                                 basicProperties: properties,
                                 body: body);

            return true;
        }
    }
}
