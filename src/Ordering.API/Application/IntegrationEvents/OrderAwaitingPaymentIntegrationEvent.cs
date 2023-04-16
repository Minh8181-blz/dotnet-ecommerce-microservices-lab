using Application.Base.SeedWork;
using Ordering.API.Domain.Entities;

namespace API.Ordering.Application.IntegrationEvents
{
    public class OrderAwaitingPaymentIntegrationEvent : IntegrationEvent
    {
        public OrderAwaitingPaymentIntegrationEvent(Order order) : base(order, false)
        {
            OrderId = order.Id;
            CustomerId = order.CustomerId;
            Amount = order.Amount;
        }

        public int OrderId { get; }
        public decimal Amount { get; }
        public int CustomerId { get; }
    }
}
