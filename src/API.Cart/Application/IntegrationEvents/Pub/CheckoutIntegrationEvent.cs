using API.Carts.Application.Dto;
using Application.Base.SeedWork;
using System.Collections.Generic;

namespace API.Carts.Application.IntegrationEvents.Pub
{
    public class CheckoutIntegrationEvent : IntegrationEvent
    {
        public int CustomerId { get; }
        public List<CheckoutItemDto> Items { get; }
        public string Description { get; }

        public CheckoutIntegrationEvent(int customerId, List<CheckoutItemDto> items, string description)
        {
            CustomerId = customerId;
            Items = items;
            Description = description;
        }
    }
}
