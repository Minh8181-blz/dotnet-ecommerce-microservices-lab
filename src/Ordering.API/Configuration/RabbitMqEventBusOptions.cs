using API.Ordering.Application.IntegrationEvents;
using BB.EventBus.RabbitMQ.Enums;
using BB.EventBus.RabbitMQ.Models;
using Microsoft.Extensions.Configuration;
using Ordering.API.Application.IntegrationEvents;
using System.Collections.Generic;

namespace API.Ordering.Configuration
{
    public class RabbitMqEventBusOptions
    {
        public static EventBusOptions GetOptions(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("QueueConnection");
            var options = new EventBusOptions
            {
                ConnectionString = connectionString,
                ConsumerSpecifications = new List<ConsumerSpecification>
                {
                    ConsumerSpecification.CreateSpecification<DraftPaymentCreatedIntegrationEvent>(
                        ConsumerTypeEnum.Basic, "q.paymentms_orderms_on_order_ev", "test_micro.order", "int_event.draft_payment_created"),
                    ConsumerSpecification.CreateSpecification<OrderPaidIntegrationEvent>(
                        ConsumerTypeEnum.Basic, "q.paymentms_orderms_on_order_ev", "test_micro.payment", "int_event.order_paid")
                },
                PublisherSpecifications = new List<PublisherSpecification>
                {
                    PublisherSpecification.CreateSpecification<OrderCreatedIntegrationEvent>(
                        PublisherTypeEnum.Basic, "test_micro.order", "int_event.order_created")
                }
            };

            return options;
        }
    }
}
