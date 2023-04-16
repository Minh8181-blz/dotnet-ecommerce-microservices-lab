using API.Ordering.Application.Dto;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using MediatR;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Application.Dto;
using Ordering.API.Domain.Entities;
using Ordering.API.Domain.Interfaces;
using Service.CommunicationStandard.Const;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using API.Ordering.Application;
using IIntegrationEventBus = BB.EventBus.Interfaces.IIntegrationEventBus;

namespace Ordering.API.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderCreationResultDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegrationEventBus integrationEventBus;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IIntegrationEventService integrationEventService,
            IUnitOfWork unitOfWork,
            IIntegrationEventBus integrationEventBus)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _integrationEventService = integrationEventService;
            _unitOfWork = unitOfWork;
            this.integrationEventBus = integrationEventBus;
        }

        public async Task<OrderCreationResultDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var result = new OrderCreationResultDto();

            if (request.CustomerId == 0 || request.OrderItems == null || !request.OrderItems.Any())
            {
                result.Code = ActionCode.BadCommand;
                result.Message = "Invalid order data";
                return result;
            }

            var orderItems = await CreateOrderItemsAsync(request.OrderItems);
            if (orderItems == null || orderItems.Count() < request.OrderItems.Count())
            {
                result.Code = OrderingActionCode.InvalidOrderItem;
                return result;
            }

            var order = Order.CreateOrder(request.CustomerId, request.Description, orderItems);

            await _unitOfWork.BeginTransactionAsync();

            _orderRepository.Add(order);
            await _unitOfWork.SaveEntitiesAsync();

            var @event = new OrderCreatedIntegrationEvent(order);
            //await _integrationEventService.SaveEventAsync(@event);
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitAsync();

            //await _integrationEventService.PublishAsync(@event);
            integrationEventBus.PublishEvent(@event);

            result.Succeeded = true;
            result.Code = ActionCode.Created;
            result.Order = new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Amount = order.Amount,
                Description = order.Description,
                Status = order.Status,
                StatusText = order.StatusText,
                CreatedAt = order.CreatedAt,
                LastUpdatedAt = order.LastUpdatedAt
            };

            return result;
        }

        private async Task<IEnumerable<OrderItem>> CreateOrderItemsAsync(IEnumerable<OrderItemDto> orderItemDtos)
        {
            var productIds = orderItemDtos.Select(x => x.ProductId);

            var products = await _productRepository.GetByIdsAsync(productIds);

            return products?.Select(x => OrderItem.CreateOrderItem(x, orderItemDtos.First(y => y.ProductId == x.Id).Quantity));
        }
    }

    public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, OrderCreationResultDto>
    {
        public CreateOrderIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager)
            : base(mediator, requestManager)
        {
        }

        protected override OrderCreationResultDto CreateResultForDuplicateRequest()
        {
            var result = new OrderCreationResultDto
            {
                Succeeded = false,
                Code = ActionCode.DuplicateCommand,
                Message = "Request is already handled"
            };

            return result;
        }
    }
}
