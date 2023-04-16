using API.Ordering.Application.IDaos;
using MediatR;
using Ordering.API.Application.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace API.Ordering.Application.Queries
{
    public class GetOrderDetailsQueryHandler : IRequestHandler<GetOrderDetailsQuery, OrderDto>
    {
        private readonly IOrdersDao _ordersDao;

        public GetOrderDetailsQueryHandler(IOrdersDao ordersDao)
        {
            _ordersDao = ordersDao;
        }

        public async Task<OrderDto> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _ordersDao.GetCustomerOrderDetails(request.CustomerId, request.Id);
        }
    }
}
