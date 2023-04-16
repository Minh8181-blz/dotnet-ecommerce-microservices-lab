using API.Carts.Application.Dto;
using MediatR;

namespace API.Carts.Application.Commands
{
    public class AddToCartCommand : IRequest<CartUpdateResultDto>
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public AddToCartCommand(int customerId, int productId, int quantity)
        {
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
