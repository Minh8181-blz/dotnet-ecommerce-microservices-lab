using MediatR;
using Ordering.API.Application.Dto;

namespace API.Ordering.Application.Queries
{
    public class GetOrderDetailsQuery : IRequest<OrderDto>
    {
        public GetOrderDetailsQuery(int customerId, int id)
        {
            CustomerId = customerId;
            Id = id;
        }

        public int CustomerId { get; }
        public int Id { get; }
    }
}
