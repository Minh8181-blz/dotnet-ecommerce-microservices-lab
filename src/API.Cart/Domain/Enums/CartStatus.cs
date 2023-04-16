using Domain.Base.SeedWork;

namespace API.Carts.Domain.Enums
{
    public class CartStatus : Enumeration
    {
        public static CartStatus Active = new CartStatus(1, "active");
        public static CartStatus Abandoned = new CartStatus(2, "abandoned");
        public static CartStatus Locked = new CartStatus(3, "locked");
        public static CartStatus Unloaded = new CartStatus(4, "unloaded");

        public CartStatus(int id, string name) : base(id, name)
        {
        }
    }
}
