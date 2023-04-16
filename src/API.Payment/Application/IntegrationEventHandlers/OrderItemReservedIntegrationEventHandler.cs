using API.Payment.Application.Commands;
using API.Payment.Application.IntegrationEvents;
using API.Payment.Domain.Enums;
using Application.Base.SeedWork;
using Infrastructure.Base.EventBus;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Payment.Application.IntegrationEventHandlers
{
    public class OrderItemReservedIntegrationEventHandler : IRequestHandler<IntegrationEventNotification<OrderItemReservedIntegrationEvent>, bool>
    {
        private readonly IMediator _mediator;

        public OrderItemReservedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(IntegrationEventNotification<OrderItemReservedIntegrationEvent> request, CancellationToken cancellationToken)
        {
            if (request.Data.Succeeded)
            {
                var order = request.Data.Order;
                var command = new CreateDraftPaymentCommand(order.CustomerId, PaymentPurpose.OrderPurchase, order.Id, order.Amount);
                var identifiedCommand = new IdentifiedCommand<CreateDraftPaymentCommand, bool>(command, request.Data.Id);

                return await _mediator.Send(identifiedCommand, cancellationToken);
            }

            return true;
        }
    }
}
