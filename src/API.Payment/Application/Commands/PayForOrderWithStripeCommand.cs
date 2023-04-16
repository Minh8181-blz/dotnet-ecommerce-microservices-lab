using API.Payment.Application.Dto;
using MediatR;

namespace API.Payment.Application.Commands
{
    public class PayForOrderWithStripeCommand : IRequest<PayForOrderWithStripeResultDto>
    {
        public PayForOrderWithStripeCommand(int customerId, int orderId, string successUrl, string cancelUrl)
        {
            CustomerId = customerId;
            OrderId = orderId;
            SuccessUrl = successUrl;
            CancelUrl = cancelUrl;
        }

        public int CustomerId { get; }
        public int OrderId { get; }
        public string SuccessUrl { get; }
        public string CancelUrl { get; }
    }
}
