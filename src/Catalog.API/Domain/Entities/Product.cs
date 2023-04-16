using Domain.Base.SeedWork;
using System;

namespace API.Catalog.Domain.Entities
{
    public class Product : Entity<int>, IAggregateRoot
    {
        public Product(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
    }
}
