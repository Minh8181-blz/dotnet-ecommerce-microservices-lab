using API.Ordering.Application.Dto;
using MediatR;
using Ordering.API.Application.Dto;
using System;
using System.Collections.Generic;

namespace Ordering.API.Application.Commands
{
    public class CreateOrderCommand : IRequest<OrderCreationResultDto>
    {
        public CreateOrderCommand(int customerId, IEnumerable<OrderItemDto> orderItems, string description)
        {
            CustomerId = customerId;
            OrderItems = orderItems;
            CreatedAt = DateTime.UtcNow;
            Description = description;
        }

        public int CustomerId { get; }
        public IEnumerable<OrderItemDto> OrderItems { get; }
        public DateTime CreatedAt { get; }
        public string Description { get; }
    }
}
