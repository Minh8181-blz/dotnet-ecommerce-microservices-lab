using API.Ordering.Application.IntegrationEvents;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.API.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace API.Ordering.Application.Commands
{
    public class UpdateOrderToAwaitingShipmentCommandHandler : IRequestHandler<UpdateOrderToAwaitingShipmentCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly ILogger<UpdateOrderToAwaitingShipmentCommandHandler> _logger;

        public UpdateOrderToAwaitingShipmentCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IIntegrationEventService integrationEventService,
            ILogger<UpdateOrderToAwaitingShipmentCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _integrationEventService = integrationEventService;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateOrderToAwaitingShipmentCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);

            try
            {
                order.UpdateStatusToAwaitingShipment();

                _orderRepository.Update(order);

                var @event = new OrderAwaitingShipmentIntegrationEvent(order);
                await _integrationEventService.SaveEventAsync(@event);

                await _unitOfWork.SaveEntitiesAsync();
                await _integrationEventService.PublishAsync(@event);
            }
            catch (DomainException ex)
            {
                _logger.LogTrace(ex, ex.Message);
            }

            return true;
        }
    }

    public class UpdateOrderToAwaitingShipmentIdentifiedCommandHandler : IdentifiedCommandHandler<UpdateOrderToAwaitingShipmentCommand, bool>
    {
        public UpdateOrderToAwaitingShipmentIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager)
            : base(mediator, requestManager)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
