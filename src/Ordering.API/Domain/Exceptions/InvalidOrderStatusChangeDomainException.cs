using Domain.Base.SeedWork;
using Ordering.API.Domain.Enums;

namespace API.Ordering.Domain.Exceptions
{
    public class InvalidOrderStatusChangeDomainException : DomainException
    {
        public InvalidOrderStatusChangeDomainException(int orderId, OrderStatus currentStatus, OrderStatus intendStatus)
            : base($"Invalid intended status change for order {orderId}: {currentStatus.Name} to {intendStatus.Name}")
        {
            OrderId = orderId;
            CurrentStatus = currentStatus;
            IntendStatus = intendStatus;
        }

        public int OrderId { get; }
        public OrderStatus CurrentStatus { get; }
        public OrderStatus IntendStatus { get; }
    }
}
