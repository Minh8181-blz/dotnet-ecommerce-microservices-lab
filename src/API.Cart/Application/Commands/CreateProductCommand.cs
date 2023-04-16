using MediatR;

namespace API.Carts.Application.Commands
{
    public class CreateProductCommand : IRequest<bool>
    {
        public CreateProductCommand(int productId, string productName, decimal productPrice)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
        }

        public int ProductId { get;}
        public string ProductName { get; }
        public decimal ProductPrice { get; }
    }
}
