using System.Collections.Generic;

namespace Plugin.Stripe.Models.ParamModels
{
    public class StripeSessionCreateModel
    {
        public string Name { get; set; }
        public string ClientReferenceId { get; set; }
        public List<string> PaymentMethods { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
