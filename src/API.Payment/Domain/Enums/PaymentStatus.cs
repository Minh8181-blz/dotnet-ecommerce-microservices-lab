using Domain.Base.SeedWork;

namespace API.Payment.Domain.Enums
{
    public class PaymentStatus : Enumeration
    {
        public static readonly PaymentStatus Pending = new PaymentStatus(1, "pending");
        public static readonly PaymentStatus Paid = new PaymentStatus(2, "paid");
        public static readonly PaymentStatus Refunded = new PaymentStatus(3, "refunded");
        public static readonly PaymentStatus Void = new PaymentStatus(4, "void");
        public static readonly PaymentStatus PartiallyRefunded = new PaymentStatus(5, "partially_refunded");
        public static readonly PaymentStatus Cancelled = new PaymentStatus(5, "cancelled");

        public PaymentStatus(int id, string name) : base(id, name)
        {

        }
    }
}
