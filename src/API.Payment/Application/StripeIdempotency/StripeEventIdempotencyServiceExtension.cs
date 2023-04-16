using API.Payment.Infrastructure.StripeIdempotency;
using Microsoft.Extensions.DependencyInjection;

namespace API.Payment.Application.StripeIdempotency
{
    public static class StripeEventIdempotencyServiceExtension
    {
        public static void UseStripeIdempotencyWithDatabase<T>(this IServiceCollection services) where T : class, IStripeEventManagerDbContext
        {
            services.AddScoped<IStripeEventManager, StripeEventManager<T>>();
        }
    }
}
