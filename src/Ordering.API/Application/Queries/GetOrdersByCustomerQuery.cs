using API.Ordering.Application.Dto.Filters;
using MediatR;
using Ordering.API.Application.Dto;
using System.Collections.Generic;
using Utilities.Pagination;

namespace API.Ordering.Application.Queries
{
    public class GetOrdersByCustomerQuery : IRequest<PaginationDataModel<OrderDto>>
    {
        public GetOrdersByCustomerQuery(OrdersByCustomerPaginationModel paginationModel)
        {
            PaginationModel = paginationModel;
        }

        public OrdersByCustomerPaginationModel PaginationModel { get; }
    }
}
