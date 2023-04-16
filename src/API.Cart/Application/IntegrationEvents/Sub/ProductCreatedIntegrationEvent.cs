using Application.Base.SeedWork;

namespace API.Carts.Application.IntegrationEvents.Sub
{
    public class ProductCreatedIntegrationEvent : IntegrationEvent
    {
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal ProductPrice { get; private set; }
    }
}
