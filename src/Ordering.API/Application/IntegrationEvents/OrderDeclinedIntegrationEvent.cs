using Application.Base.SeedWork;
using Ordering.API.Domain.Entities;

namespace API.Ordering.Application.IntegrationEvents
{
    public class OrderDeclinedIntegrationEvent : IntegrationEvent
    {
        public OrderDeclinedIntegrationEvent(Order order) : base(order, false)
        {
            Order = order;
        }

        public Order Order { get; }
    }
}
