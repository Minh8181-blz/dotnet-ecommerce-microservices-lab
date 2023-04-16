using API.Payment.Application.Dto;
using Application.Base.SeedWork;

namespace API.Payment.Application.IntegrationEvents
{
    public class OrderItemReservedIntegrationEvent : IntegrationEvent
    {
        public OrderDto Order { get; set; }
        public bool Succeeded { get; private set; }
    }
}
