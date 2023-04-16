using Domain.Base.SeedWork;

namespace API.Ordering.Domain.Enums
{
    public class ReserveFailureReason : Enumeration
    {
        public static ReserveFailureReason OutOfStock = new ReserveFailureReason(1, "out-of-stock");

        public ReserveFailureReason(int id, string name) : base(id, name)
        {
        }
    }
}
