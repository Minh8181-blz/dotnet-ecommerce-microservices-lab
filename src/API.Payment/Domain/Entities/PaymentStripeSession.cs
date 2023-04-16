using API.Payment.Domain.Enums;
using Domain.Base.SeedWork;
using System;

namespace API.Payment.Domain.Entities
{
    public class PaymentStripeSession : Entity<Guid>
    {
        public static PaymentStripeSession CreatePaymentStripeSession(Guid paymentId, string sessionId, string refId, string successUrl, string cancelUrl)
        {
            var paymentReference = new PaymentStripeSession
            {
                PaymentOperationId = paymentId,
                SessionId = sessionId,
                ClientReference = refId,
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                Status = PaymentStatus.Pending.Id
            };

            return paymentReference;
        }

        public Guid PaymentOperationId { get; private set; }
        public string SessionId { get; private set; }
        public string ClientReference { get; private set; }
        public string SuccessUrl { get; private set; }
        public string CancelUrl { get; private set; }
        public int Status { get; private set; }
        public string StatusText { get; private set; }

        public void Cancel()
        {
            UpdateStatus(PaymentStatus.Cancelled);
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsPaid()
        {
            UpdateStatus(PaymentStatus.Paid);
            LastUpdatedAt = DateTime.UtcNow;
        }

        private void UpdateStatus(PaymentStatus paymentStatus)
        {
            Status = paymentStatus.Id;
            StatusText = paymentStatus.Name;
        }
    }
}
