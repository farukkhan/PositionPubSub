﻿using Application.Interfaces;
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
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            logger.LogInformation($"Message received: {jsonMessage}");

            eventRepository.PersistEvent(context.Message);

            return Task.CompletedTask;
        }
    }
}
