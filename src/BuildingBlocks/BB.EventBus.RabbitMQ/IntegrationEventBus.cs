using BB.EventBus.Events;
using BB.EventBus.Interfaces;
using BB.EventBus.Models;
using BB.EventBus.RabbitMQ.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BB.EventBus.RabbitMQ
{
    class IntegrationEventBus : IIntegrationEventBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConsumerManager _consumerManager;
        private readonly PublisherManager _publisherManager;

        public IntegrationEventBus(
            IServiceProvider serviceProvider,
            ConsumerManager consumerManager,
            PublisherManager publisherManager)
        {
            _serviceProvider = serviceProvider;
            _consumerManager = consumerManager;
            _publisherManager = publisherManager;
        }

        public bool PublishEvent(IntegrationEvent @event)
        {
            var eventType = @event.GetType();
            var publisher = _publisherManager.GetPublisherForEventType(eventType);

            if (publisher == null)
            {
                throw new Exception("No publisher for this event");
            }

            publisher.Publish(@event);
            return true;
        }

        public bool SubscribeEvent<T>() where T : IntegrationEvent
        {
            var eventType = typeof(T);
            var consumer = _consumerManager.GetConsumerForEventType(eventType);
            if (consumer == null)
            {
                throw new Exception("No consumer for this event");
            }

            consumer.ActivateSubscription(eventType, Consume);
            return true;
        }

        private bool Consume<T>(T @event) where T : IntegrationEvent
        {
            var eventType = @event.GetType();

            Type notificationType = typeof(IntegrationEventNotification<>).MakeGenericType(eventType);

            var notification = Activator.CreateInstance(notificationType, @event);

            using var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();

            var result = mediator.Send(notification).Result as bool?;

            return result.Value;
        }
    }
}
