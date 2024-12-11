using Application.Interfaces;
using IntergrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class PositionCreatedEventConsumer(
        IEventRepository eventRepository,
        ILogger<PositionCreatedEventConsumer> logger) : IConsumer<PositionCreatedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<PositionCreatedIntegrationEvent> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message); //This will make the process slow
            logger.LogInformation($"Message received: {jsonMessage}"); //This will make the process slow

            eventRepository.PersistEvent(context.Message);

            return Task.CompletedTask;
        }
    }
}
