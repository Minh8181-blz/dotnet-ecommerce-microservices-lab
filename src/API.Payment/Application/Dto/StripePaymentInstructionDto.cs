using API.Payment.Domain.Entities;
using API.Payment.Domain.Enums;
using Plugin.Stripe.Models;

namespace API.Payment.Application.Dto
{
    public class StripePaymentInstructionDto : PaymentInstructionDto
    {
        public StripePaymentInstructionDto(PaymentOperation payment, StripeSession session) : base(payment, PaymentMethod.Stripe.Id)
        {
            SessionId = session.Id;
            Url = session.Url;
        }

        public string SessionId { get; }
        public string Url { get; }
    }
}
