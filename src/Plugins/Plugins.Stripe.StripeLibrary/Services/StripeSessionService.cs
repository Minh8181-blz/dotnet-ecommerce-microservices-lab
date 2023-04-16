using Plugin.Stripe.Extensions.StripeNet;
using Plugin.Stripe.Models;
using Plugin.Stripe.Models.ParamModels;
using Plugin.Stripe.Services.Intefaces;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Stripe.StripeLibrary.Services
{
    public class StripeSessionService : IStripeSessionService
    {
        public async Task<StripeSession> CreateStripeSessionAsync(StripeSessionCreateModel model)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = model.PaymentMethods,
                    LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = Convert.ToInt64(model.Amount * 100),
                            Currency = model.Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = model.Name,
                            },
                        },
                        Quantity = 1,
                    },
                },
                    ClientReferenceId = model.ClientReferenceId,
                    Mode = "payment",
                    SuccessUrl = model.SuccessUrl + "?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = model.CancelUrl + "?session_id={CHECKOUT_SESSION_ID}"
                };
                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                var stripeSession = session.ToStripeSession();
                return stripeSession;
            }
            catch (StripeException ex)
            {
                var outerException = ex.ToStripePluginException();
                throw outerException;
            }
        }
    }
}
