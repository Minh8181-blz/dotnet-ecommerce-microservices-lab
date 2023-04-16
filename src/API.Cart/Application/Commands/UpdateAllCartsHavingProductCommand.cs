using MediatR;

namespace API.Carts.Application.Commands
{
    public class UpdateAllCartsHavingProductCommand : IRequest<bool>
    {
        public UpdateAllCartsHavingProductCommand(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; }
    }
}
