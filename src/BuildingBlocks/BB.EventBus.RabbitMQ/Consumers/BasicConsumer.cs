using BB.EventBus.Events;
using BB.EventBus.RabbitMQ.MessageConverter.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace BB.EventBus.RabbitMQ.Consumers
{
    class BasicConsumer : Consumer
    {
        public BasicConsumer(
            IConnection connection,
            string queue,
            IMessageConverter deserializer,
            ushort? prefetchCount = null,
            bool multipleAck = false)
            : base(connection, queue, deserializer, prefetchCount, multipleAck)
        {
        }

        private EventingBasicConsumer _consumerInstance;
        protected EventingBasicConsumer ConsumerInstance
        {
            get
            {
                if (_consumerInstance == null)
                {
                    if (_prefetchCount.HasValue)
                    {
                        _channel.BasicQos(0, _prefetchCount.Value, false);
                    }
                    _consumerInstance = new EventingBasicConsumer(_channel);
                }

                return _consumerInstance;
            }
        }

        public bool IsConsuming { get; protected set; }
        protected override void Consume(Func<IntegrationEvent, bool> onReceiptCallback)
        {
            if (IsConsuming)
                return;

            ConsumerInstance.Received += (model, ea) =>
            {
                var eventType = GetActiveEventType(ea.Exchange, ea.RoutingKey);
                if (eventType == null)
                {
                    _channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);
                }

                var body = Deserialize(ea.Body, eventType);
                bool result = onReceiptCallback.Invoke(body);

                if (result)
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: _multipleAck);
                else
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
            };

            _channel.BasicConsume(queue: Queue, autoAck: false, consumer: ConsumerInstance);
            IsConsuming = true;
        }
    }
}
