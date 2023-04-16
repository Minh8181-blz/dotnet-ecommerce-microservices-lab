using API.Ordering.Application.Dto;
using API.Ordering.Application.IntegrationEvents;
using Application.Base.SeedWork;
using Infrastructure.Base.EventBus;
using MediatR;
using Ordering.API.Application.Commands;
using Ordering.API.Application.Dto;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Ordering.Application.IntegrationEventHandlers
{
    public class CheckoutIntegrationEventHandler : IRequestHandler<IntegrationEventNotification<CheckoutIntegrationEvent>, bool>
    {
        private readonly IMediator _mediator;

        public CheckoutIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(IntegrationEventNotification<CheckoutIntegrationEvent> request, CancellationToken cancellationToken)
        {
            var customerId = request.Data.CustomerId;
            var checkoutItems = request.Data.Items;

            if (customerId > 0 && checkoutItems != null && checkoutItems.Any())
            {
                var orderItems = checkoutItems.Select(x => new OrderItemDto {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                });

                var command = new CreateOrderCommand(customerId, orderItems, request.Data.Description);
                var identifiedCommand = new IdentifiedCommand<CreateOrderCommand, OrderCreationResultDto>(command, request.Data.Id);

                await _mediator.Send(identifiedCommand, cancellationToken);
            }

            return true;
        }
    }
}
