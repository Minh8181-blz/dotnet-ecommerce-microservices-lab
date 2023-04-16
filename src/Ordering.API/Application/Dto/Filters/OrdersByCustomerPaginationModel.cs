using Utilities.Pagination;

namespace API.Ordering.Application.Dto.Filters
{
    public class OrdersByCustomerPaginationModel : PaginationModel
    {
        public OrdersByCustomerPaginationModel(int customerId, int pageSize, int pageIndex)
        {
            CustomerId = customerId;
            PageSize = pageSize > 0 ? pageSize : 0;
            PageIndex = pageIndex > 1 ? pageIndex : 1;
        }

        public int CustomerId { get; set; }
    }
}
