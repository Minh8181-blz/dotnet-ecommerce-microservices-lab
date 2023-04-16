using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Ordering.Application.Dto;
using API.Ordering.Application.Dto.Filters;
using API.Ordering.Application.Queries;
using Application.Base.SeedWork;
using Base.API.Filters.ActionFilters;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Application.Commands;
using Ordering.API.Application.Dto;
using Ordering.API.ViewModels;
using Utilities.Pagination;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ValidateModel]
        [ValidateRequestId]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateOrder(OrderViewModel model, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            var orderItems = model.OrderItems?.Select(x => new OrderItemDto
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            });

            model.CustomerId = int.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.Id).Value);

            var command = new CreateOrderCommand(model.CustomerId, orderItems, model.Description);
            var identifiedCommand = new IdentifiedCommand<CreateOrderCommand, OrderCreationResultDto>(command, requestId);

            var result = await _mediator.Send(identifiedCommand);

            if (result == null || !result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("my-orders")]
        [Authorize(Roles = "Customer")]
        public async Task<PaginationDataModel<OrderDto>> GetMyOrders([FromQuery]int pageSize, [FromQuery] int pageIndex)
        {
            if (pageSize <= 0 || pageIndex <= 0)
                return null;

            var customerId = int.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.Id).Value);
            var paginationModel = new OrdersByCustomerPaginationModel(customerId, pageSize, pageIndex);
            var query = new GetOrdersByCustomerQuery(paginationModel);

            return await _mediator.Send(query);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> GetOrderDetails(int id)
        {
            var customerId = int.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.Id).Value);
            var query = new GetOrderDetailsQuery(customerId, id);

            var order = await _mediator.Send(query);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}
