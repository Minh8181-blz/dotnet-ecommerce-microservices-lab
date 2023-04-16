using API.Catalog.Application.IntegrationEvents;
using Infrastructure.Base.EventBus;
using System.Collections.Generic;

namespace API.Catalog.Infrastructure
{
    public class IntegrationEventTopicMapping : IIntegrationEventTopicMapping
    {
        // catalog exchange
        public const string CatalogExchange = "test_micro.catalog";

        public const string ProductCreatedPubRoutingKey = "int_event.product_created";

        // order exchange
        public const string OrderExchange = "test_micro.order";

        public const string OrderCreatedSubRoutingKey = "int_event.order_created";

        public const string OrderItemReservedPubRoutingKey = "int_event.order_item_reserved";

        public const string OrderServiceOnOrderEventSubQueue = "q.orderms_catalogms_on_order_ev";


        // pub map
        private readonly Dictionary<string, RabbitMqTopicModel> PubMaps = new Dictionary<string, RabbitMqTopicModel>
        {
            {
                nameof(ProductCreatedIntegrationEvent), new RabbitMqTopicModel(CatalogExchange, ProductCreatedPubRoutingKey)
            },
            {
                nameof(OrderItemReservedIntegrationEvent), new RabbitMqTopicModel(OrderExchange, OrderItemReservedPubRoutingKey)
            }
        };


        // sub map
        private readonly Dictionary<string, RabbitMqQueueModel> SubMaps = new Dictionary<string, RabbitMqQueueModel>
        {
            {
                nameof(OrderCreatedIntegrationEvent), new RabbitMqQueueModel(OrderExchange, OrderCreatedSubRoutingKey, OrderServiceOnOrderEventSubQueue)
            }
        };

        public RabbitMqTopicModel GetPublishedTopic(string eventTypeName)
        {
            if (PubMaps.ContainsKey(eventTypeName))
            {
                return PubMaps[eventTypeName];
            }

            return null;
        }

        public RabbitMqQueueModel GetSubscribedQueue(string eventTypeName)
        {
            if (SubMaps.ContainsKey(eventTypeName))
            {
                return SubMaps[eventTypeName];
            }

            return null;
        }
    }
}
