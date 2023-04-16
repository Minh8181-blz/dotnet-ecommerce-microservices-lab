using System;

namespace Website.MarketingSite.Models.ViewModels.Payment
{
    public class StripePaymentInstructionViewModel : PaymentInstructionViewModel
    {
        public string SessionId { get; set; }
        public string Url { get; set; }
    }
}
