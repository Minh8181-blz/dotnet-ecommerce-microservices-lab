using API.Ordering.Application.Dto.Filters;
using API.Ordering.Application.IDaos;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Application.Dto;
using Ordering.API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Pagination;

namespace API.Ordering.Infrastructure.Daos
{
    public class OrdersDao : IOrdersDao
    {
        private readonly OrderingContext _context;

        public OrdersDao(OrderingContext context)
        {
            _context = context;
        }

        public async Task<PaginationDataModel<OrderDto>> GetOrdersByCustomerAsync(OrdersByCustomerPaginationModel paginationModel)
        {
            int pageIndex = paginationModel.PageIndex;
            int? pageSize = paginationModel.PageSize;

            var query = _context.Orders
                .AsNoTracking()
                .Where(x => x.CustomerId == paginationModel.CustomerId);

            var result = new PaginationDataModel<OrderDto>
            {
                Total = await query.CountAsync()
            };

            if (pageSize.HasValue && pageSize.Value > 0)
            {
                result.TotalPages = Convert.ToInt32(Math.Ceiling((decimal)result.Total / pageSize.Value));

                result.List = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((pageIndex - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .Select(x => new OrderDto
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        Amount = x.Amount,
                        Description = x.Description,
                        Status = x.Status,
                        StatusText = x.StatusText,
                        CreatedAt = x.CreatedAt,
                        LastUpdatedAt = x.LastUpdatedAt
                    })
                    .ToListAsync();
            }
            else
            {
                result.List = new List<OrderDto>();
            }

            return result;
        }

        public async Task<OrderDto> GetCustomerOrderDetails(int customerId, int id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .Select(x => new OrderDto
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    Amount = x.Amount,
                    Description = x.Description,
                    Status = x.Status,
                    StatusText = x.StatusText,
                    CreatedAt = x.CreatedAt,
                    LastUpdatedAt = x.LastUpdatedAt,
                    OrderItems = x.OrderItems.Select(x => new OrderItemDto
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.Id == id);

            return order;
        }
    }
}
