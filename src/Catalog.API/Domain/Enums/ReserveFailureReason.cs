using Domain.Base.SeedWork;

namespace API.Catalog.Domain.Enums
{
    public class ReserveFailureReason : Enumeration
    {
        public static ReserveFailureReason OutOfStock = new ReserveFailureReason(1, "out-of-stock");
        public static ReserveFailureReason InsufficientQuantity = new ReserveFailureReason(2, "insufficient-quantity");

        public ReserveFailureReason(int id, string name) : base(id, name)
        {
        }
    }
}
