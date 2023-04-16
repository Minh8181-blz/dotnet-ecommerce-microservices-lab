using MediatR;

namespace API.Ordering.Application.Commands
{
    public class UpdateOrderToDeclinedCommand : IRequest<bool>
    {
        public UpdateOrderToDeclinedCommand(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; }
    }
}
