using System.Collections.Generic;

namespace API.Carts.Application.Dto
{
    public class CartDto
    {
        public int CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public ICollection<CartItemDto> CartItems { get; set; }
    }
}
