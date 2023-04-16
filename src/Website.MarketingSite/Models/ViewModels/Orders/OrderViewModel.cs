using System;
using System.Collections.Generic;

namespace Website.MarketingSite.Models.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public decimal Amount { get; set; }
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
    }
}
