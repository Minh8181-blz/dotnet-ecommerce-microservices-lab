using API.Carts.Application.Dto;
using API.Carts.Application.Interfaces;
using API.Carts.Domain.Entities;
using API.Carts.Domain.Interfaces;
using Domain.Base.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using Service.CommunicationStandard.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Carts.Application.Commands
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartUpdateResultDto>
    {
        private readonly ICartsRepository _cartRepository;
        private readonly IProductDao _productDao;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddToCartCommandHandler> _logger;

        public AddToCartCommandHandler(
            ICartsRepository cartsRepository,
            IProductDao productDao,
            IUnitOfWork unitOfWork,
            ILogger<AddToCartCommandHandler> logger)
        {
            _cartRepository = cartsRepository;
            _productDao = productDao;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CartUpdateResultDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetActiveCartByCustomerAsync(request.CustomerId);

            if (cart == null)
                return await CreateNewCartAsync(request);

            if (cart.HasProductItems(request.ProductId))
                return await UpdateCartItemQuantityAsync(request, cart);
            else
                return await AddNewItemToCartAsync(request, cart);
        }

        private async Task<CartUpdateResultDto> CreateNewCartAsync(AddToCartCommand request)
        {
            var result = new CartUpdateResultDto();

            var product = await _productDao.GetAsync(request.ProductId);
            if (product == null)
            {
                FormatResultDtoOnNonExistenceProduct(result, request.ProductId);
                return result;
            }

            var cartItem = new CartItem(product.Id, product.Name, product.Price, request.Quantity, product.PictureUrl);
            if (!cartItem.IsValid())
            {
                FormatResultDtoOnInvalidCartItem(result);
                return result;
            }

            var cart = new Cart(request.CustomerId, new List<CartItem> { cartItem });
            _cartRepository.Add(cart);
            await _unitOfWork.SaveEntitiesAsync();
            FormatResultDtoOnSuccess(result, cart, true);

            return result;
        }

        private async Task<CartUpdateResultDto> UpdateCartItemQuantityAsync(AddToCartCommand request, Cart cart)
        {
            var result = new CartUpdateResultDto();

            cart.UpdateItemQuantity(request.ProductId, request.Quantity);
            _cartRepository.Update(cart);
            await _unitOfWork.SaveEntitiesAsync();
            FormatResultDtoOnSuccess(result, cart, false);

            return result;
        }

        private async Task<CartUpdateResultDto> AddNewItemToCartAsync(AddToCartCommand request, Cart cart)
        {
            var result = new CartUpdateResultDto();

            var product = await _productDao.GetAsync(request.ProductId);

            if (product == null)
            {
                FormatResultDtoOnNonExistenceProduct(result, request.ProductId);
                _logger.LogError(result.Message);
                return result;
            }

            var cartItem = new CartItem(product.Id, product.Name, product.Price, request.Quantity, product.PictureUrl);

            if (!cartItem.IsValid())
            {
                FormatResultDtoOnInvalidCartItem(result);
                return result;
            }

            cart.AddItem(cartItem);
            _cartRepository.Update(cart);
            await _unitOfWork.SaveEntitiesAsync();
            FormatResultDtoOnSuccess(result, cart, false);

            return result;
        }

        private void FormatResultDtoOnNonExistenceProduct(CartUpdateResultDto result, int productId)
        {
            result.Code = ActionCode.Error;
            result.Message = string.Format("Product with id {0} does not exist", productId);
            _logger.LogError(result.Message);
        }

        private void FormatResultDtoOnInvalidCartItem(CartUpdateResultDto result)
        {
            result.Code = ActionCode.BadCommand;
            result.Message = string.Format("Invalid cart data");
        }

        private void FormatResultDtoOnSuccess(CartUpdateResultDto result, Cart cart, bool isCreated)
        {
            result.Succeeded = true;
            result.Code = isCreated ? ActionCode.Created : ActionCode.Success;
            result.Cart = new CartDto
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
        }
    }
}
