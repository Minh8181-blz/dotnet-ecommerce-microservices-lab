using API.Carts.Application.IntegrationEvents.Sub;
using Application.Base.SeedWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API.Carts.Application
{
    public class EventSubscriber
    {
        public static void RegisterIntegrationEventSubscription(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IIntegrationEventBus>();
            eventBus.SubscribeEvent<ProductCreatedIntegrationEvent>();
        }
    }
}
