using API.Payment.Application.IntegrationEvents;
using Infrastructure.Base.EventBus;

namespace API.Payment.Infrastructure
{
    public class IntegrationEventTopicMapping : IntegrationEventTopicMappingBase
    {
        // payment exchange
        public const string PaymentExchange = "test_micro.payment";

        public const string OrderPaidPubRoutingKey = "int_event.order_paid";

        // order exchange
        public const string OrderExchange = "test_micro.order";

        public const string DraftPaymentCreatedPubRoutingKey = "int_event.draft_payment_created";
        public const string OrderItemReservedSubRoutingKey = "int_event.order_item_reserved";

        public const string OrderServiceOnOrderEventSubQueue = "q.orderms_catalogms_on_order_ev";
        public const string CatalogServiceOnOrderItemReservedSubQueue = "q.catalogms_paymentms_order_item_reserved";

        public IntegrationEventTopicMapping()
        {
            PubMaps.Add(nameof(DraftPaymentCreatedIntegrationEvent), new RabbitMqTopicModel(OrderExchange, DraftPaymentCreatedPubRoutingKey));
            PubMaps.Add(nameof(OrderPaidIntegrationEvent), new RabbitMqTopicModel(PaymentExchange, OrderPaidPubRoutingKey));

            SubMaps.Add(nameof(OrderItemReservedIntegrationEvent),
                new RabbitMqQueueModel(OrderExchange, OrderItemReservedSubRoutingKey, CatalogServiceOnOrderItemReservedSubQueue));
        }
    }
}
