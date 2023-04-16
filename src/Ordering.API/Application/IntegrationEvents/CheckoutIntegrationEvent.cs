using API.Ordering.Application.Dto;
using Application.Base.SeedWork;
using System.Collections.Generic;

namespace API.Ordering.Application.IntegrationEvents
{
    public class CheckoutIntegrationEvent : IntegrationEvent
    {
        public int CustomerId { get; set; }
        public List<CheckoutItemDto> Items { get; set; }
        public string Description { get; set; }
    }
}
