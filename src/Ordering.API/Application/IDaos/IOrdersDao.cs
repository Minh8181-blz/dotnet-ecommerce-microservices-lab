using API.Ordering.Application.Dto.Filters;
using Ordering.API.Application.Dto;
using System.Threading.Tasks;
using Utilities.Pagination;

namespace API.Ordering.Application.IDaos
{
    public interface IOrdersDao
    {
        Task<PaginationDataModel<OrderDto>> GetOrdersByCustomerAsync(OrdersByCustomerPaginationModel paginationModel);
        Task<OrderDto> GetCustomerOrderDetails(int customerId, int id);
    }
}
