using API.Catalog.Application.Dto;
using MediatR;

namespace API.Catalog.Application.Commands
{
    public class CreateProductCommand : IRequest<ProductAdminDto>
    {
        public CreateProductCommand(string name, string description, decimal price, int stockQuantity)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
        public int StockQuantity { get; }
    }
}
