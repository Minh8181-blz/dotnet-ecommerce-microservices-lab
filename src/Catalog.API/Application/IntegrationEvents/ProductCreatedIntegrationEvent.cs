using API.Catalog.Domain.Entities;
using Application.Base.SeedWork;

namespace API.Catalog.Application.IntegrationEvents
{
    public class ProductCreatedIntegrationEvent : IntegrationEvent
    {
        public ProductCreatedIntegrationEvent(Product product, bool entityHasBeenCreated) : base(product, entityHasBeenCreated)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            ProductPrice = product.Price;
        }

        public int ProductId { get; }
        public string ProductName { get; }
        public decimal ProductPrice { get; }
    }
}
