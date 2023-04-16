namespace Website.MarketingSite.Models.ViewModels.Payment
{
    public class PayForOrderStripeResultViewModel
    {
        public bool Succeeded { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public StripePaymentInstructionViewModel PaymentInstruction { get; set; }
    }
}
