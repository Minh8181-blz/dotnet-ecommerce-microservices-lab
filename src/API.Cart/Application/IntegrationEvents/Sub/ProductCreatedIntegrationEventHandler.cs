using API.Carts.Application.Commands;
using Infrastructure.Base.EventBus;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Carts.Application.IntegrationEvents.Sub
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
            await _mediator.Send(command, cancellationToken);
            
            return true;
        }
    }
}
