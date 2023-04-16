using API.Ordering.Application.IntegrationEvents;
using Application.Base.SeedWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ordering.API.Application.IntegrationEvents;

namespace Ordering.API.Application
{
    public class EventSubscriber
    {
        public static void RegisterIntegrationEventSubscription(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IIntegrationEventBus>();

            eventBus.SubscribeEvent<PriceCalculatedIntegrationEvent>();
            eventBus.SubscribeEvent<ProductCreatedIntegrationEvent>();
            eventBus.SubscribeEvent<OrderItemReservedIntegrationEvent>();
            //eventBus.SubscribeEvent<DraftPaymentCreatedIntegrationEvent>();
            //eventBus.SubscribeEvent<OrderPaidIntegrationEvent>();
            eventBus.SubscribeEvent<CheckoutIntegrationEvent>();
        }
    }
}
