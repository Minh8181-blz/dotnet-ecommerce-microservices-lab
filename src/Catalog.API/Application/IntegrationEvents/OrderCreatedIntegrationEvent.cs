using API.Catalog.Application.Dto;
using Application.Base.SeedWork;

namespace API.Catalog.Application.IntegrationEvents
{
    public class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public OrderDto Order { get; set; }
    }
}
