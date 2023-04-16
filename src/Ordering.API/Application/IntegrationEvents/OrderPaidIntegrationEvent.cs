using BB.EventBus.Events;

namespace API.Ordering.Application.IntegrationEvents
{
    public class OrderPaidIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; private set; }
        public decimal PaidAmount { get; private set; }
    }
}
