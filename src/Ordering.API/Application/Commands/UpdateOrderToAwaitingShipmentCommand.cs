using MediatR;

namespace API.Ordering.Application.Commands
{
    public class UpdateOrderToAwaitingShipmentCommand : IRequest<bool>
    {
        public UpdateOrderToAwaitingShipmentCommand(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; }
    }
}
