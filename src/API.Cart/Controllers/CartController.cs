using API.Carts.Application.Commands;
using API.Carts.Application.Dto;
using API.Carts.Application.Queries;
using API.Carts.Domain.Entities;
using API.Carts.Domain.Interfaces;
using API.Carts.ViewModels;
using Base.API.Filters.ActionFilters;
using Grpc.Net.Client;
using Grpc.Pricing.Protos;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Carts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("my-cart")]
        public async Task<CartDto> GetCart()
        {
            var customerId = int.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.Id).Value);
            var query = new GetCustomerCartQuery(customerId);

            var cartDto = await _mediator.Send(query);
            return cartDto;
        }

        [HttpPost("add-to-cart")]
        [ValidateModel]
        public async Task<ActionResult> AddToCart(AddToCartViewModel model)
        {
            var customerId = int.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.Id).Value);
            var command = new AddToCartCommand(customerId, model.ProductId, model.Quantity);

            var result = await _mediator.Send(command);

            if (result == null || !result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("update-cart-details")]
        public async Task<ActionResult> UpdateCart(UpdateCartViewModel model)
        {
            var customerId = int.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.Id).Value);

            var command = new UpdateCartCommand(customerId, model.ProductId, model.Action);

            var result = await _mediator.Send(command);

            if (result == null || !result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("test-grpc")]
        public async Task<OrderPriceViewModel> TestGrpc()
        {
            List<OrderItem> items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 1.3
                },
                new OrderItem
                {
                    ProductId = 2,
                    Quantity = 3,
                    UnitPrice = 1
                }
            };
            var order = new OrderViewModel();
            order.Items.AddRange(items);

            var channel = GrpcChannel.ForAddress("https://localhost:6005");
            var client = new CalculateOrderPrice.CalculateOrderPriceClient(channel);

            var price = await client.CalculateAsync(order);
            return price;
        }
    }
}
