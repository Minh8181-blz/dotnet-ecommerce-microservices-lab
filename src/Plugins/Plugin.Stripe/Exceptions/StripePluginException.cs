using Stripe;
using System;

namespace Plugin.Stripe.Exceptions
{
    public class StripePluginException : Exception
    {
        public StripePluginException(string message) : base(message)
        {

        }
    }
}
