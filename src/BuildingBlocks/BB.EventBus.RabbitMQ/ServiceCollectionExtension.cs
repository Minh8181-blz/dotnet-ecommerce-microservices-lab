using BB.EventBus.Interfaces;
using BB.EventBus.RabbitMQ.Consumers;
using BB.EventBus.RabbitMQ.MessageConverter;
using BB.EventBus.RabbitMQ.Models;
using BB.EventBus.RabbitMQ.Publishers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace BB.EventBus.RabbitMQ
{
    public static class ServiceCollectionExtension
    {
        public static void UseRabbitMqEventBus(this IServiceCollection services, EventBusOptions options)
        {
            if (options == null)
            {
                throw new Exception("EventBus RabbitMQ: Invalid passed option");
            }

            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(options.ConnectionString)
            };

            var connection = connectionFactory.CreateConnection();
            services.AddSingleton(connection);

            services.AddTransient<ConsumerFactory>();
            services.AddTransient<PublisherFactory>();
            services.AddTransient<MessageConverterFactory>();

            services.AddSingleton(serviceProvider => new ConsumerManager(options.ConsumerSpecifications, serviceProvider));
            services.AddSingleton(serviceProvider => new PublisherManager(options.PublisherSpecifications, serviceProvider));
            services.AddSingleton<IIntegrationEventBus, IntegrationEventBus>();
        }
    }
}
