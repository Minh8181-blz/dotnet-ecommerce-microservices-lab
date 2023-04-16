using API.Ordering.Application.Dto;
using BB.EventBus.Events;

namespace API.Ordering.Application.IntegrationEvents
{
    public class DraftPaymentCreatedIntegrationEvent : IntegrationEvent
    {
        public PaymentOperationDto Payment { get; set; }
    }
}
