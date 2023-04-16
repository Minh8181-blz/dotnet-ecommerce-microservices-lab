using API.Carts.Application.Dto;
using API.Carts.Domain.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Carts.Application.Queries
{
    public class GetCustomerCartQueryHandler : IRequestHandler<GetCustomerCartQuery, CartDto>
    {
        private readonly ICartsRepository _cartsRepository;

        public GetCustomerCartQueryHandler(ICartsRepository cartsRepository)
        {
            _cartsRepository = cartsRepository;
        }

        public async Task<CartDto> Handle(GetCustomerCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartsRepository.GetActiveCartByCustomerAsync(request.CustomerId);

            if (cart == null)
                return null;

            var cartDto = new CartDto
            {
                CustomerId = cart.CustomerId,
                Status = cart.Status,
                StatusText = cart.StatusText,
                TotalPrice = cart.TotalPrice,
                CartItems = cart.CartItems?.Select(x => new CartItemDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    PictureUrl = x.PictureUrl,
                    Quantity = x.Quantity
                })?.ToList()
            };

            return cartDto;
        }
    }
}
