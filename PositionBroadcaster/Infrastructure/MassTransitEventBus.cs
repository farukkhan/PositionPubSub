using Application.Interfaces;
using IntergrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class MassTransitEventBus(IPublishEndpoint publishEndpoint, ILogger<MassTransitEventBus> logger) : IEventBus
    {
        public async Task PublishAsync<T>(T @event) where T : IntegrationEventBase
        {
            await publishEndpoint.Publish(@event);

            var jsonEvent = JsonConvert.SerializeObject(@event) ?? String.Empty;
            logger.LogInformation(jsonEvent);
        }
    }
}
