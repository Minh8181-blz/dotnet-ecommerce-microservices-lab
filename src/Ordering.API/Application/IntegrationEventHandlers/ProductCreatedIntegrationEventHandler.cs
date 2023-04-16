using API.Ordering.Application.Commands;
using API.Ordering.Application.IntegrationEvents;
using Application.Base.SeedWork;
using Infrastructure.Base.EventBus;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Ordering.Application.IntegrationEventHandlers
{
    public class ProductCreatedIntegrationEventHandler : IRequestHandler<IntegrationEventNotification<ProductCreatedIntegrationEvent>, bool>
    {
        private readonly IMediator _mediator;

        public ProductCreatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(IntegrationEventNotification<ProductCreatedIntegrationEvent> request, CancellationToken cancellationToken)
        {
            var command = new CreateProductCommand(request.Data.ProductId, request.Data.ProductName, request.Data.ProductPrice);
            var identifiedCommand = new IdentifiedCommand<CreateProductCommand, bool>(command, request.Data.Id);

            return await _mediator.Send(identifiedCommand, cancellationToken);
        }
    }
}
