using Domain.Base.SeedWork;
using System;

namespace Ordering.API.Domain.Entities
{
    public class OrderItem : Entity<int>
    {
        public static OrderItem CreateOrderItem(Product product, int quantity)
        {
            var item = new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = quantity,
                SubTotal = product.Price * quantity,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            return item;
        }

        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal SubTotal { get; private set; }
    }
}
