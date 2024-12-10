using Application.Interfaces;
using Domain.Events;
using MassTransit;

namespace Infrastructure
{
    public class MassTransitEventBus(IPublishEndpoint publishEndpoint) : IEventBus
    {
        public async Task PublishAsync<T>(T @event) where T : EventBase
        {
            await publishEndpoint.Publish(@event);
        }
    }
}
