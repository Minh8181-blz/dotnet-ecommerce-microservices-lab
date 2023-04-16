using Plugin.Stripe.Models;
using Stripe.Checkout;

namespace Plugin.Stripe.Extensions.StripeNet
{
    public static class StripeNetSessionExtensions
    {
        public static StripeSession ToStripeSession(this Session session)
        {
            var stripeSession = new StripeSession
            {
                Id = session.Id,
                ClientReferenceId = session.ClientReferenceId,
                PaymentIntentId = session.PaymentIntentId,
                Url = session.Url,
                CustomerId = session.CustomerId
            };

            return stripeSession;
        }
    }
}
