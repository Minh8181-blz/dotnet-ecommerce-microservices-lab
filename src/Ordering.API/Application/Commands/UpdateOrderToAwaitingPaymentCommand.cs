using MediatR;

namespace API.Ordering.Application.Commands
{
    public class UpdateOrderToAwaitingPaymentCommand : IRequest<bool>
    {
        public UpdateOrderToAwaitingPaymentCommand(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; }
    }
}
