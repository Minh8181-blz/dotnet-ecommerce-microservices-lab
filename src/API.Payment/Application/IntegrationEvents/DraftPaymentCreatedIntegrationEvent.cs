using API.Payment.Domain.Entities;
using Application.Base.SeedWork;

namespace API.Payment.Application.IntegrationEvents
{
    public class DraftPaymentCreatedIntegrationEvent : IntegrationEvent
    {
        public DraftPaymentCreatedIntegrationEvent(PaymentOperation payment)
        {
            Payment = payment;
        }

        public PaymentOperation Payment { get; }
    }
}
