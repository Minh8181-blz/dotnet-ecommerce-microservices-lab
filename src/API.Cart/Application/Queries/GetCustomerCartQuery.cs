using API.Carts.Application.Dto;
using MediatR;

namespace API.Carts.Application.Queries
{
    public class GetCustomerCartQuery : IRequest<CartDto>
    {
        public int CustomerId { get; set; }

        public GetCustomerCartQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
