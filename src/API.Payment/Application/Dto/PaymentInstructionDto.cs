using API.Payment.Domain.Entities;
using System;

namespace API.Payment.Application.Dto
{
    public abstract class PaymentInstructionDto
    {
        public PaymentInstructionDto(PaymentOperation payment, int paymentMethodId)
        {
            PaymentId = payment.Id;
            CustomerId = payment.CustomerId;
            MethodId = paymentMethodId;
        }

        public int MethodId { get; set; }
        public Guid PaymentId { get; set; }
        public int CustomerId { get; set; }
    }
}
