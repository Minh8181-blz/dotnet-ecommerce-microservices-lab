using API.Catalog.Application.Dto;
using API.Catalog.Application.IntegrationEvents;
using API.Catalog.Domain.Entities;
using API.Catalog.Domain.Enums;
using API.Catalog.Domain.Exceptions;
using API.Catalog.Domain.Interfaces;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Catalog.Application.Commands
{
    public class ReserveProductCommandHandler : IRequestHandler<ReserveProductCommand, bool>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReserveProductCommandHandler> _logger;

        public ReserveProductCommandHandler(
            IStockRepository stockRepository,
            IIntegrationEventService integrationEventService,
            IUnitOfWork unitOfWork,
            ILogger<ReserveProductCommandHandler> logger)
        {
            _stockRepository = stockRepository;
            _integrationEventService = integrationEventService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(ReserveProductCommand request, CancellationToken cancellationToken)
        {
            var productIds = request.Order.OrderItems.Select(x => x.ProductId);
            var stocks = await _stockRepository.GetByProductIdsAsync(productIds);
            var reserveResults = new List<OrderItemReserveResult>();
            var failedStockIds = new List<int>();

            foreach (var item in request.Order.OrderItems)
            {
                var reserveResult = new OrderItemReserveResult();

                var stock = stocks.FirstOrDefault(x => x.ProductId == item.ProductId);

                if (stock == null)
                {
                    failedStockIds.Add(stock.Id);
                    reserveResult.Code = ReserveFailureReason.OutOfStock.Name;
                }
                else
                {
                    try
                    {
                        stock.ReserveItem(request.Order.Id, item.Quantity);
                        reserveResult.Succeeded = true;
                    }
                    catch (InsufficientStockItemsDomainException ex)
                    {
                        failedStockIds.Add(stock.Id);
                        reserveResult.Code = ReserveFailureReason.InsufficientQuantity.Name;
                        _logger.LogInformation(ex.Message);
                    }

                    _stockRepository.Update(stock);
                }
                reserveResults.Add(reserveResult);
            }

            bool reserveSuccess = !failedStockIds.Any();
            if (!reserveSuccess)
            {
                foreach (var stock in stocks)
                {
                    stock.UnreserveItem(request.Order.Id);
                    _stockRepository.Update(stock);
                }
            }

            var @event = new OrderItemReservedIntegrationEvent(request.Order, reserveSuccess, reserveResults);
            await _integrationEventService.SaveEventAsync(@event);
            await _unitOfWork.SaveEntitiesAsync();
            await _integrationEventService.PublishAsync(@event);

            return true;
        }
    }

    public class ReserveProductIdentifiedCommandHandler : IdentifiedCommandHandler<ReserveProductCommand, bool>
    {
        public ReserveProductIdentifiedCommandHandler(
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
