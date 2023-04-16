using System.Collections.Generic;

namespace Website.MarketingSite.Models.ViewModels.Cart
{
    public class CartViewModel
    {
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public ICollection<CartItemViewModel> CartItems { get; set; }
    }

    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }
}
