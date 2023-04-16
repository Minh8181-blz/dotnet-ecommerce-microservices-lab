using API.Ordering.Application.Commands;
using API.Ordering.Application.IntegrationEvents;
using Application.Base.SeedWork;
using BB.EventBus.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Ordering.Application.IntegrationEventHandlers
{
    public class DraftPaymentCreatedIntegrationEventHandler : IRequestHandler<IntegrationEventNotification<DraftPaymentCreatedIntegrationEvent>, bool>
    {
        private readonly IMediator _mediator;

        public DraftPaymentCreatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(IntegrationEventNotification<DraftPaymentCreatedIntegrationEvent> request, CancellationToken cancellationToken)
        {
            var payment = request.Data.Payment;
            if (payment.OrderId.HasValue)
            {
                var command = new UpdateOrderToAwaitingPaymentCommand(payment.OrderId.Value);
                var identifiedCommand = new IdentifiedCommand<UpdateOrderToAwaitingPaymentCommand, bool>(command, request.Data.Id);

                return await _mediator.Send(identifiedCommand, cancellationToken);
            }

            return true;
        }
    }
}
