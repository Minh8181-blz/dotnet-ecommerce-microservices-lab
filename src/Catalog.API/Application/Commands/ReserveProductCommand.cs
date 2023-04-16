using API.Catalog.Application.Dto;
using MediatR;

namespace API.Catalog.Application.Commands
{
    public class ReserveProductCommand : IRequest<bool>
    {
        public ReserveProductCommand(OrderDto order)
        {
            Order = order;
        }

        public OrderDto Order { get; }
    }
}
