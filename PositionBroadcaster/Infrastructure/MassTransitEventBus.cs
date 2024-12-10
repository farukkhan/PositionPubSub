using Application.Interfaces;
using IntergrationEvents;
using MassTransit;

namespace Infrastructure
{
    public class MassTransitEventBus(IPublishEndpoint publishEndpoint) : IEventBus
    {
        public async Task PublishAsync<T>(T @event) where T : IntegrationEventBase
        {
            await publishEndpoint.Publish(@event);
        }
    }
}
