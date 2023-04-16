using Domain.Base.SeedWork;

namespace API.Carts.Domain.Enums
{
    public class CartItemStatus : Enumeration
    {
        public static CartItemStatus Active = new CartItemStatus(1, "active");
        public static CartItemStatus Removed = new CartItemStatus(2, "removed");

        public CartItemStatus(int id, string name) : base(id, name)
        {
        }
    }
}
