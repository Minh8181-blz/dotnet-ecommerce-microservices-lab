using API.Payment.Domain.Enums;
using Domain.Base.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Payment.Domain.Entities
{
    public class PaymentOperation : Entity<Guid>, IAggregateRoot
    {
        protected PaymentOperation() { }

        public int CustomerId { get; private set; }
        public int PurposeId { get; private set; }
        public int? OrderId { get; private set; }
        public decimal Amount { get; private set; }
        public decimal? AmountRefunded { get; private set; }
        public int Status { get; private set; }
        public string StatusText { get; private set; }
        public virtual ICollection<PaymentStripeSession> StripeSessions { get; private set; }

        public static PaymentOperation CreateDraftPayment(int customerId, PaymentPurpose purpose, int? orderId, decimal amount)
        {
            if (purpose.Equals(PaymentPurpose.OrderPurchase) && !orderId.HasValue)
            {
                throw new DomainException("Order purchase payment does not have order reference");
            }

            var payment = new PaymentOperation
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                PurposeId = purpose.Id,
                OrderId = orderId,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            payment.UpdateStatus(PaymentStatus.Pending);

            return payment;
        }

        public void InitStripePayment(string sessionId, string refId, string successUrl, string cancelUrl)
        {
            if (Status != PaymentStatus.Pending.Id)
            {
                throw new DomainException("Invalid payment status");
            }

            if (StripeSessions == null)
            {
                StripeSessions = new List<PaymentStripeSession>();
            }

            var session = PaymentStripeSession.CreatePaymentStripeSession(Id, sessionId, refId, successUrl, cancelUrl);
            StripeSessions.Add(session);
        }

        public void CompleteStripePaymentForOrder(string sessionId)
        {
            if (Status != PaymentStatus.Pending.Id)
            {
                throw new DomainException(string.Format("Payment {0}: Invalid payment status", Id));
            }

            var session = StripeSessions?.FirstOrDefault(x => x.SessionId == sessionId);

            if (session == null)
            {
                throw new DomainException(string.Format("Payment {0}: Invalid Stripe payment session id {1}", Id, sessionId));
            }

            UpdateStatus(PaymentStatus.Paid);
            LastUpdatedAt = DateTime.UtcNow;

            session.MarkAsPaid();
        }

        private void UpdateStatus(PaymentStatus status)
        {
            Status = status.Id;
            StatusText = status.Name;
        }
    }
}
