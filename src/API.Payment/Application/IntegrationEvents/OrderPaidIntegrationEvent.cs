using Application.Base.SeedWork;

namespace API.Payment.Application.IntegrationEvents
{
    public class OrderPaidIntegrationEvent : IntegrationEvent
    {
        public OrderPaidIntegrationEvent(int orderId, decimal paidAmount)
        {
            OrderId = orderId;
            PaidAmount = paidAmount;
        }

        public int OrderId { get; }
        public decimal PaidAmount { get; }
    }
}
