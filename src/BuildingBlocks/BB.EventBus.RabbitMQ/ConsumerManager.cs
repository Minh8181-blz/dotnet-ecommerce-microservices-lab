using BB.EventBus.RabbitMQ.Consumers;
using BB.EventBus.RabbitMQ.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BB.EventBus.RabbitMQ
{
    class ConsumerManager
    {
        private readonly ConsumerFactory _consumerFactory;
        private readonly List<Consumer> _consumers;

        public ConsumerManager(
            IEnumerable<ConsumerSpecification> consumerSpecifications,
            IServiceProvider serviceProvider)
        {
            _consumerFactory = serviceProvider.GetRequiredService<ConsumerFactory>();
            _consumers = new List<Consumer>();
            InitList(consumerSpecifications);
        }

        private void InitList(IEnumerable<ConsumerSpecification> consumerSpecifications)
        {
            try
            {
                foreach (var spec in consumerSpecifications)
                {
                    var consumer = _consumers.FirstOrDefault(x => x.Queue == spec.Queue);
                    if (consumer == null)
                    {
                        consumer = Create(spec);
                    }

                    consumer.BindEvent(spec.EventType, spec.Exchange, spec.RoutingKey);
                }
            }
            catch
            {
                throw new Exception("EventBus RabbitMQ: Invalid passed consumer specification");
            }
        }

        public Consumer Create(ConsumerSpecification specification)
        {
            var consumer = _consumers.FirstOrDefault(x => x.Queue == specification.Queue);
            if (consumer != null)
            {
                throw new Exception($"Consumer for {specification.Queue} already exists");
            }

            consumer = DoCreate(specification);
            _consumers.Add(consumer);
            return consumer;
        }

        private Consumer DoCreate(ConsumerSpecification specification)
        {
            var consumer = _consumerFactory.Create(specification);
            return consumer;
        }

        public Consumer GetConsumerForEventType(Type eventType)
        {
            return _consumers.FirstOrDefault(x => x.Subscriptions.Any(x => x.EventType.Equals(eventType)));
        }
    }
}
