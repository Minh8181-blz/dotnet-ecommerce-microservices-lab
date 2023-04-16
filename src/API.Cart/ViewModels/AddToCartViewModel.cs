using System;
using System.ComponentModel.DataAnnotations;

namespace API.Carts.ViewModels
{
    public class AddToCartViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid product")]
        public int ProductId { get; set; }
        [Range(1, 100, ErrorMessage = "Invalid quantity")]
        public int Quantity { get; set; }
    }
}
