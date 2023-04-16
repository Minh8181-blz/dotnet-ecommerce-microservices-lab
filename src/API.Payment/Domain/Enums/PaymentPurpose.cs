using Domain.Base.SeedWork;

namespace API.Payment.Domain.Enums
{
    public class PaymentPurpose : Enumeration
    {
        public static readonly PaymentPurpose OrderPurchase = new PaymentPurpose(1, "order_purchase");
        public static readonly PaymentPurpose DepositToWallet = new PaymentPurpose(2, "deposit_to_wallet");

        public PaymentPurpose(int id, string name) : base(id, name)
        {

        }
    }
}
