using Application.Base.SeedWork;

namespace API.Payment.Application.Dto
{
    public class PayForOrderWithStripeResultDto : CommandResultModel
    {
        public StripePaymentInstructionDto PaymentInstruction { get; set; }
    }
}
