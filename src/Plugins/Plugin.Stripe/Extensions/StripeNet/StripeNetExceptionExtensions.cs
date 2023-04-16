using Plugin.Stripe.Exceptions;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Stripe.Extensions.StripeNet
{
    public static class StripeNetExceptionExtensions
    {
        public static StripePluginException ToStripePluginException(this StripeException ex)
        {
            var exception = new StripePluginException(ex.Message);
            return exception;
        }
    }
}
