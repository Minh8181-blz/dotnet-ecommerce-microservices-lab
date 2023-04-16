using API.Carts.Application.Dto;
using API.Carts.Application.Enums;
using MediatR;

namespace API.Carts.Application.Commands
{
    public class UpdateCartCommand : IRequest<CartUpdateResultDto>
    {
        public int CustomerId { get; }
        public int ProductId { get; }
        public CartAction Action { get; }

        public UpdateCartCommand(int customerId, int productId, CartAction action)
        {
            CustomerId = customerId;
            ProductId = productId;
            Action = action;
        }
    }
}
