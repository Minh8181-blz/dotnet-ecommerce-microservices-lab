using Application.Base.SeedWork;
using Ordering.API.Domain.Entities;

namespace API.Ordering.Application.IntegrationEvents
{
    public class ProductCreatedIntegrationEvent : IntegrationEvent
    {
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal ProductPrice { get; private set; }
    }
}
