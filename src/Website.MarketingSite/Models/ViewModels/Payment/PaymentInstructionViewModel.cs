using System;

namespace Website.MarketingSite.Models.ViewModels.Payment
{
    public class PaymentInstructionViewModel
    {
        public int MethodId { get; set; }
        public Guid PaymentId { get; set; }
        public int CustomerId { get; set; }
    }
}
