using API.Ordering.Application.IntegrationEvents;
using BB.EventBus.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API.Ordering.Application
{
    public static class IntegrationEventSubscriber
    {
        public static void RegisterIntegrationEventSubscription(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IIntegrationEventBus>();

            eventBus.SubscribeEvent<DraftPaymentCreatedIntegrationEvent>();
            eventBus.SubscribeEvent<OrderPaidIntegrationEvent>();
        }
    }
}
