using Microsoft.Extensions.DependencyInjection;
using Plugin.Stripe.Services.Intefaces;
using Plugin.Stripe.StripeLibrary.Services;
using Stripe;

namespace Plugin.Stripe.StripeLibrary
{
    public static class StripePluginServiceExtensions
    {
        public static void UseStripePlugin(this IServiceCollection services, string secretApiKey)
        {
            StripeConfiguration.ApiKey = secretApiKey;
            services.AddTransient<IStripeSessionService, StripeSessionService>();
        }
    }
}
