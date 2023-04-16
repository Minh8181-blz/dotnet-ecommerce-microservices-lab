using MessagePack;

namespace API.Carts.Infrastructure.MessagePack
{
    [MessagePackObject]
    public class CartItemMessagePackModel
    {
        [Key(0)]
        public int ProductId { get; set; }
        [Key(1)]
        public string ProductName { get; set; }
        [Key(2)]
        public decimal UnitPrice { get; set; }
        [Key(3)]
        public int Quantity { get; set; }
        [Key(4)]
        public string PictureUrl { get; set; }
    }
}
