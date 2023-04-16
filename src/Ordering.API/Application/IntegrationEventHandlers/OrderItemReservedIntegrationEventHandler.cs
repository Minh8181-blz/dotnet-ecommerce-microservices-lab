using API.Ordering.Application.Commands;
using API.Ordering.Application.IntegrationEvents;
using Application.Base.SeedWork;
using Infrastructure.Base.EventBus;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Ordering.Application.IntegrationEventHandlers
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
            var command = new UpdateOrderOnItemReservedCommand(request.Data.Order, request.Data.Succeeded, request.Data.ReserveResults);
            var identifiedCommand = new IdentifiedCommand<UpdateOrderOnItemReservedCommand, bool>(command, request.Data.Id);

            return await _mediator.Send(identifiedCommand, cancellationToken);
        }
    }
}
