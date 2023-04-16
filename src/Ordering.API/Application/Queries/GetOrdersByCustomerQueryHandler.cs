using API.Ordering.Application.IDaos;
using MediatR;
using Ordering.API.Application.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Pagination;

namespace API.Ordering.Application.Queries
{
    public class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, PaginationDataModel<OrderDto>>
    {
        private readonly IOrdersDao _ordersDao;

        public GetOrdersByCustomerQueryHandler(IOrdersDao ordersDao)
        {
            _ordersDao = ordersDao;
        }

        public async Task<PaginationDataModel<OrderDto>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
        {
            return await _ordersDao.GetOrdersByCustomerAsync(request.PaginationModel);
        }
    }
}
