using System;
using System.Collections.Generic;

namespace API.Catalog.Application.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public int Status { get; set; }
        public IEnumerable<OrderItemDto> OrderItems { get; set; }
    }
}
