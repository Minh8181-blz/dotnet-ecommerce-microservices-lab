using Domain.Base.SeedWork;
using System;

namespace Ordering.API.Domain.Entities
{
    public class Product : Entity<int>, IAggregateRoot
    {
        public Product(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public decimal Price { get; private set; }
        public string Name { get; private set; }
    }
}
