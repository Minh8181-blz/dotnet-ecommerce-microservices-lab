using MessagePack;
using System.Collections.Generic;

namespace API.Carts.Infrastructure.MessagePack
{
    [MessagePackObject]
    public class CartMessagePackModel
    {
        [Key(0)]
        public int CustomerId { get; set; }
        [Key(1)]
        public decimal TotalPrice { get; set; }
        [Key(2)]
        public List<CartItemMessagePackModel> Items { get; set; }
    }
}
