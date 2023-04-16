using Stripe.Checkout;

namespace Plugin.Stripe.Models
{
    public partial class StripeSession
    {
        public string Id { get; set; }
        public string ClientReferenceId { get; set; }
        public string PaymentIntentId { get; set; }
        public string Url { get; set; }
        public string CustomerId { get; set; }

        public StripeSession() { }
    }
}
