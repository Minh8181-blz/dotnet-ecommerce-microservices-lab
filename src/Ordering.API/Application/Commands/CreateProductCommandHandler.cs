using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using MediatR;
using Ordering.API.Domain.Entities;
using Ordering.API.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace API.Ordering.Application.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(request.ProductId, request.ProductName, request.ProductPrice);
            _productRepository.Add(product);
            return await _unitOfWork.SaveEntitiesAsync();
        }
    }

    public class CreateProductIdentifiedCommandHandler : IdentifiedCommandHandler<CreateProductCommand, bool>
    {
        public CreateProductIdentifiedCommandHandler(
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
