using API.Ordering.Application.Commands;
using API.Ordering.Application.IntegrationEvents;
using Application.Base.SeedWork;
using BB.EventBus.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Ordering.Application.IntegrationEventHandlers
{
    public class OrderPaidIntegrationEventHandler : IRequestHandler<IntegrationEventNotification<OrderPaidIntegrationEvent>, bool>
    {
        private readonly IMediator _mediator;

        public OrderPaidIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(IntegrationEventNotification<OrderPaidIntegrationEvent> request, CancellationToken cancellationToken)
        {
            var command = new UpdateOrderToAwaitingShipmentCommand(request.Data.OrderId);
            var identifiedCommand = new IdentifiedCommand<UpdateOrderToAwaitingShipmentCommand, bool>(command, request.Data.Id);

            return await _mediator.Send(identifiedCommand, cancellationToken);
        }
    }
}
