using API.Ordering.Application.Dto;
using MediatR;
using Ordering.API.Application.Dto;
using System.Collections.Generic;

namespace API.Ordering.Application.Commands
{
    public class UpdateOrderOnItemReservedCommand : IRequest<bool>
    {
        public UpdateOrderOnItemReservedCommand(OrderDto order, bool succeeded, IEnumerable<OrderItemReserveResultDto> reserveResults)
        {
            Order = order;
            Succeeded = succeeded;
            ReserveResults = reserveResults;
        }

        public OrderDto Order { get; set; }
        public bool Succeeded { get; private set; }
        public IEnumerable<OrderItemReserveResultDto> ReserveResults { get; private set; }
    }
}
