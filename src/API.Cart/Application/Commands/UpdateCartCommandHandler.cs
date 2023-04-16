using API.Carts.Application.Dto;
using API.Carts.Application.Enums;
using API.Carts.Application.Interfaces;
using API.Carts.Domain.Entities;
using API.Carts.Domain.Enums;
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
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, CartUpdateResultDto>
    {
        private readonly ICartsRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCartCommandHandler> _logger;

        public UpdateCartCommandHandler(
            ICartsRepository cartsRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateCartCommandHandler> logger)
        {
            _cartRepository = cartsRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<CartUpdateResultDto> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetActiveCartByCustomerAsync(request.CustomerId);

            if (cart == null)
                return CreateResultDtoOnInvalidCart();

            if (request.Action == CartAction.AddItemQuantity)
                return await AddCartItemQuantityAsync(request, cart);

            if (request.Action == CartAction.DecreaseItemQuantity)
                return await DecreaseCartItemQuantityAsync(request, cart);

            if (request.Action == CartAction.RemoveItem)
                return await RemoveCartItemAsync(request, cart);

            if (request.Action == CartAction.Abandon)
                return await AbandonCartAsync(cart);

            return CreateResultDtoOnInvalidCart();
        }

        private CartUpdateResultDto CreateResultDtoOnInvalidCart()
        {
            var result = new CartUpdateResultDto
            {
                Code = ActionCode.BadCommand,
                Message = string.Format("Invalid cart")
            };

            return result;
        }

        private async Task<CartUpdateResultDto> AddCartItemQuantityAsync(UpdateCartCommand request, Cart cart)
        {
            var result = new CartUpdateResultDto();

            cart.UpdateItemQuantity(request.ProductId, 1);
            _cartRepository.Update(cart);
            await _unitOfWork.SaveEntitiesAsync();
            FormatResultDtoOnSuccess(result, cart, false);

            return result;
        }

        private async Task<CartUpdateResultDto> DecreaseCartItemQuantityAsync(UpdateCartCommand request, Cart cart)
        {
            var result = new CartUpdateResultDto();

            cart.UpdateItemQuantity(request.ProductId, -1);
            _cartRepository.Update(cart);
            await _unitOfWork.SaveEntitiesAsync();
            FormatResultDtoOnSuccess(result, cart, false);

            return result;
        }

        private async Task<CartUpdateResultDto> RemoveCartItemAsync(UpdateCartCommand request, Cart cart)
        {
            var result = new CartUpdateResultDto();

            cart.RemoveItem(request.ProductId);
            _cartRepository.Update(cart);
            await _unitOfWork.SaveEntitiesAsync();
            FormatResultDtoOnSuccess(result, cart, false);

            return result;
        }

        private async Task<CartUpdateResultDto> AbandonCartAsync(Cart cart)
        {
            var result = new CartUpdateResultDto();

            cart.Abandon();
            _cartRepository.Update(cart);
            await _unitOfWork.SaveEntitiesAsync();
            result.Succeeded = true;
            result.Code = ActionCode.Success;

            return result;
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
                CartItems = cart.CartItems?
                    .Where(x => x.Status == CartItemStatus.Active.Id)
                    .Select(x => new CartItemDto
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
