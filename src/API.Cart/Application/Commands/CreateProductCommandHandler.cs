using API.Carts.Application.Dto;
using API.Carts.Application.Interfaces;
using Application.Base.SeedWork;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Carts.Application.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IProductDao _productDao;

        public CreateProductCommandHandler(
            IProductDao productDao)
        {
            _productDao = productDao;
        }

        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new ProductDto
            {
                Id = request.ProductId,
                Name = request.ProductName,
                Price = request.ProductPrice,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };
            var result = await _productDao.AddAsync(product);
            return result > 0;
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
