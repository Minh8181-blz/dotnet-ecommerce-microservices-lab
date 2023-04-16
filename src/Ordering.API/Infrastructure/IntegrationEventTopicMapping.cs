using API.Ordering.Application.IntegrationEvents;
using Infrastructure.Base.EventBus;
using Ordering.API.Application.IntegrationEvents;

namespace Ordering.API.Infrastructure
{
    public class IntegrationEventTopicMapping : IntegrationEventTopicMappingBase
    {
        // order exchange
        public const string OrderExchange = "test_micro.order";

        public const string OrderCreatedPubRoutingKey = "int_event.order_created";
        public const string PriceCalculatedSubRoutingKey = "int_event.price_calculated";
        public const string OrderItemReservedSubRoutingKey = "int_event.order_item_reserved";
        public const string DraftPaymentCreatedSubRoutingKey = "int_event.draft_payment_created";
        public const string OrderAwaitingPaymentPubRoutingKey = "int_event.order_awaiting_payment";
        public const string OrderAwaitingShipmentPubRoutingKey = "int_event.order_awaiting_shipment";

        public const string PriceServiceOnOrderEventSubQueue = "q.pricems_orderms_on_order_ev";
        public const string CatalogServiceOnOrderEventSubQueue = "q.catalogms_orderms_on_order_ev";

        // catalog exchange
        public const string CatalogExchange = "test_micro.catalog";

        public const string CatalogServiceOnCatalogEventSubQueue = "q.catalogms_orderms_on_catalog_ev";

        public const string ProductCreatedSubRoutingKey = "int_event.product_created";

        // payment exchange
        public const string PaymentExchange = "test_micro.payment";

        public const string OrderPaidSubRoutingKey = "int_event.order_paid";

        public const string PaymentServiceOnOrderEventSubQueue = "q.paymentms_orderms_on_order_ev";

        // cart exchange
        public const string CartExchange = "test_micro.cart";

        public const string CheckoutSubRoutingKey = "int_event.checkout";

        public const string CartServiceOnCartEventSubQueue = "q.cartms_orderms_on_cart_ev";

        public IntegrationEventTopicMapping()
        {
            PubMaps.Add(nameof(OrderCreatedIntegrationEvent), new RabbitMqTopicModel(OrderExchange, OrderCreatedPubRoutingKey));
            PubMaps.Add(nameof(OrderAwaitingPaymentIntegrationEvent), new RabbitMqTopicModel(OrderExchange, OrderAwaitingPaymentPubRoutingKey));
            PubMaps.Add(nameof(OrderAwaitingShipmentIntegrationEvent), new RabbitMqTopicModel(OrderExchange, OrderAwaitingShipmentPubRoutingKey));

            SubMaps.Add(nameof(PriceCalculatedIntegrationEvent), new RabbitMqQueueModel(OrderExchange, PriceCalculatedSubRoutingKey, PriceServiceOnOrderEventSubQueue));
            SubMaps.Add(nameof(ProductCreatedIntegrationEvent), new RabbitMqQueueModel(CatalogExchange, ProductCreatedSubRoutingKey, CatalogServiceOnCatalogEventSubQueue));
            SubMaps.Add(nameof(OrderItemReservedIntegrationEvent), new RabbitMqQueueModel(OrderExchange, OrderItemReservedSubRoutingKey, CatalogServiceOnOrderEventSubQueue));
            SubMaps.Add(nameof(DraftPaymentCreatedIntegrationEvent), new RabbitMqQueueModel(OrderExchange, DraftPaymentCreatedSubRoutingKey, PaymentServiceOnOrderEventSubQueue));
            SubMaps.Add(nameof(OrderPaidIntegrationEvent), new RabbitMqQueueModel(PaymentExchange, OrderPaidSubRoutingKey, PaymentServiceOnOrderEventSubQueue));
            SubMaps.Add(nameof(CheckoutIntegrationEvent), new RabbitMqQueueModel(CartExchange, CheckoutSubRoutingKey, CartServiceOnCartEventSubQueue));
        }
    }
}
