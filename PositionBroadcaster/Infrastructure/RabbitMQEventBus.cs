using System.Text;
using Application.Interfaces;
using Domain.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure
{
    public sealed class RabbitMQEventBus : IEventBus
    {
        private readonly ILogger _logger;

        public RabbitMQEventBus(ILogger<RabbitMQEventBus> logger)
        {
            _logger = logger;
        }

        public async Task PublishAsync(EventBase @event)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                await using var connection = await factory.CreateConnectionAsync();
                await using var channel = await connection.CreateChannelAsync();
                var eventName = @event.GetType().Name;
                await channel.QueueDeclareAsync(eventName, true, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync("", eventName, body, default);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}