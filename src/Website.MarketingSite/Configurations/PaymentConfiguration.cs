namespace Website.MarketingSite.Configurations
{
    public class PaymentConfiguration
    {
        public StripeConfiguration Stripe { get; set; }

        public class StripeConfiguration
        {
            public string SuccessRedirectUrl { get; set; }
            public string CancelRedirectUrl { get; set; }
        }
    }
}
