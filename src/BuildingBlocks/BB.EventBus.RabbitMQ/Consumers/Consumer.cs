using BB.EventBus.Events;
using BB.EventBus.RabbitMQ.MessageConverter.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BB.EventBus.RabbitMQ.Consumers
{
    abstract class Consumer
    {
        protected readonly IConnection _connection;
        protected IModel _channel;
        protected readonly IMessageConverter _deserializer;

        protected readonly bool _multipleAck;
        protected readonly ushort? _prefetchCount;

        public Consumer(
            IConnection connection,
            string queue,
            IMessageConverter deserializer,
            ushort? prefetchCount = null,
            bool multipleAck = false)
        {
            _deserializer = deserializer;
            _prefetchCount = prefetchCount;
            _multipleAck = multipleAck;
            Queue = queue;
            Subscriptions = new List<Subscription>();
            _connection = connection;
            _channel = connection.CreateModel();
        }

        public string Queue { get; }
        public List<Subscription> Subscriptions { get; protected set; }

        protected abstract void Consume(Func<IntegrationEvent, bool> onReceiptCallback);

        public void BindEvent(Type eventType, string exchange, string routingKey)
        {
            if (Subscriptions.Any(x => x.EventType.Equals(eventType)))
            {
                return;
            }

            var queueEventTypeTuple = new Subscription
            {
                EventType = eventType,
                Exchange = exchange,
                RoutingKey = routingKey
            };

            Subscriptions.Add(queueEventTypeTuple);
        }

        public void ActivateSubscription(Type eventType, Func<IntegrationEvent, bool> onReceiptCallback)
        {
            var subscription = Subscriptions.FirstOrDefault(x => x.EventType.Equals(eventType));
            if (subscription != null && !subscription.IsActive)
            {
                subscription.IsActive = true;
                if (_channel == null)
                {
                    _channel = _connection.CreateModel();
                }

                _channel.QueueDeclare(Queue, durable: true, exclusive: false, autoDelete: false);
                _channel.ExchangeDeclare(exchange: subscription.Exchange, type: ExchangeType.Topic, durable: true);
                _channel.QueueBind(queue: Queue, exchange: subscription.Exchange, routingKey: subscription.RoutingKey);
                Consume(onReceiptCallback);
            }
        }

        protected Type GetActiveEventType(string exchange, string routingKey)
        {
            var tuple = Subscriptions.FirstOrDefault(x => x.IsActive && x.Exchange == exchange && x.RoutingKey == routingKey);
            return tuple?.EventType;
        }

        protected IntegrationEvent Deserialize(ReadOnlyMemory<byte> messageBody, Type eventType)
        {
            MethodInfo method = _deserializer.GetType().GetMethod(nameof(_deserializer.Deserialize));
            MethodInfo generic = method.MakeGenericMethod(eventType);

            var body = (IntegrationEvent)generic.Invoke(_deserializer, new object[] { messageBody });
            return body;
        }
    }

    class Subscription
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public Type EventType { get; set; }
        public bool IsActive { get; set; }
    }
}
