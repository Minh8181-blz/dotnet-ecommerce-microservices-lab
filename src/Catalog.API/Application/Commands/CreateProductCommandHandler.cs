using API.Catalog.Application.Dto;
using API.Catalog.Application.IntegrationEvents;
using API.Catalog.Domain.Entities;
using API.Catalog.Domain.Interfaces;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Catalog.Application.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductAdminDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IStockRepository stockRepository,
            IIntegrationEventService integrationEventService,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _stockRepository = stockRepository;
            _integrationEventService = integrationEventService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductAdminDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            var product = new Product(request.Name, request.Description, request.Price);

            _productRepository.Add(product);
            await _unitOfWork.SaveEntitiesAsync();

            var stock = new Stock(product.Id, request.StockQuantity);

            _stockRepository.Add(stock);
            await _unitOfWork.SaveEntitiesAsync();

            var @event = new ProductCreatedIntegrationEvent(product, true);
            await _integrationEventService.SaveEventAsync(@event);
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitAsync();

            await _integrationEventService.PublishAsync(@event);

            return new ProductAdminDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = stock.Quantity
            };
        }
    }
}
