using API.Payment.Domain.Enums;
using MediatR;

namespace API.Payment.Application.Commands
{
    public class CreateDraftPaymentCommand : IRequest<bool>
    {
        public CreateDraftPaymentCommand(int customerId, PaymentPurpose purpose, int? orderId, decimal amount)
        {
            Purpose = purpose;
            OrderId = orderId;
            CustomerId = customerId;
            Amount = amount;
        }

        public PaymentPurpose Purpose { get; }
        public int? OrderId { get; }
        public int CustomerId { get; }
        public decimal Amount { get; }
    }
}
