using Application.Interfaces;
using Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class PositionCreatedEventConsumer(
        IEventRepository eventRepository,
        ILogger<PositionCreatedEventConsumer> logger) : IConsumer<PositionCreatedEvent>
    {
        public Task Consume(ConsumeContext<PositionCreatedEvent> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"Message received: {jsonMessage}");

            eventRepository.PersistEvent(context.Message);

            return Task.CompletedTask;
        }
    }
}
