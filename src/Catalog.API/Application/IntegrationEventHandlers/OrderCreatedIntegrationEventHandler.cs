using API.Catalog.Application.Commands;
using API.Catalog.Application.IntegrationEvents;
using Application.Base.SeedWork;
using Infrastructure.Base.EventBus;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Catalog.Application.IntegrationEventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IRequestHandler<IntegrationEventNotification<OrderCreatedIntegrationEvent>, bool>
    {
        private readonly IMediator _mediator;

        public OrderCreatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(IntegrationEventNotification<OrderCreatedIntegrationEvent> request, CancellationToken cancellationToken)
        {
            var command = new ReserveProductCommand(request.Data.Order);

            var identifiedCommand = new IdentifiedCommand<ReserveProductCommand, bool>(command, request.Data.Id);

            return await _mediator.Send(identifiedCommand, cancellationToken);
        }
    }
}
