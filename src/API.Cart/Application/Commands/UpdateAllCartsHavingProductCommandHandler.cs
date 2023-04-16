using API.Carts.Application.Interfaces;
using API.Carts.Domain.Interfaces;
using Domain.Base.SeedWork;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace API.Carts.Application.Commands
{
    public class UpdateAllCartsHavingProductCommandHandler : IRequestHandler<UpdateAllCartsHavingProductCommand, bool>
    {
        private readonly ICartsRepository _cartRepository;
        private readonly IProductDao _productDao;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAllCartsHavingProductCommandHandler(
            ICartsRepository cartsRepository,
            IProductDao productDao,
            IUnitOfWork unitOfWork)
        {
            _cartRepository = cartsRepository;
            _productDao = productDao;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateAllCartsHavingProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productDao.GetAsync(request.ProductId);
            var carts = await _cartRepository.GetAllCartsHavingProductAsync(request.ProductId);

            foreach (var cart in carts)
            {
                cart.UpdateItemProductDetails(product.Id, product.Name, product.Price, product.PictureUrl);
            }

            await _unitOfWork.SaveEntitiesAsync();
            // todo: add noti to client
            return true;
        }
    }
}
