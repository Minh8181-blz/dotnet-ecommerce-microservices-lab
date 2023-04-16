using API.Payment.Application.IntegrationEvents;
using Application.Base.SeedWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API.Payment.Application
{
    public class EventSubscriber
    {
        public static void RegisterIntegrationEventSubscription(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IIntegrationEventBus>();

            eventBus.SubscribeEvent<OrderItemReservedIntegrationEvent>();
        }
    }
}
