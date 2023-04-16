using API.Carts.Application.IntegrationEvents.Pub;
using API.Carts.Application.IntegrationEvents.Sub;
using Infrastructure.Base.EventBus;

namespace API.Carts.Infrastructure
{
    public class IntegrationEventTopicMapping : IntegrationEventTopicMappingBase
    {
        // cart exchange
        public const string CartExchange = "test_micro.cart";

        public const string CheckoutPubRoutingKey = "int_event.checkout";

        // catalog exchange
        public const string CatalogExchange = "test_micro.catalog";

        public const string CatalogServiceOnCatalogEventSubQueue = "q.catalogms_cartms_on_catalog_ev";

        public const string ProductCreatedSubRoutingKey = "int_event.product_created";

        public IntegrationEventTopicMapping()
        {
            SubMaps.Add(nameof(ProductCreatedIntegrationEvent), new RabbitMqQueueModel(CatalogExchange, ProductCreatedSubRoutingKey, CatalogServiceOnCatalogEventSubQueue));

            PubMaps.Add(nameof(CheckoutIntegrationEvent), new RabbitMqTopicModel(CartExchange, CheckoutPubRoutingKey));
        }
    }
}
