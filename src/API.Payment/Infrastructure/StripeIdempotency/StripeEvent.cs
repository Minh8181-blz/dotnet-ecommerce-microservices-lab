using System;

namespace API.Payment.Infrastructure.StripeIdempotency
{
    public class StripeEvent
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; }
    }
}
