using System.Text;
using Application.Interfaces;
using Domain.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure
{
    public sealed class RabbitMQEventBus : IEventBus
    {
        public async Task PublishAsync(EventBase @event)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            var eventName = @event.GetType().Name;
            await channel.QueueDeclareAsync(@"positions", true, false, false, null);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync("", eventName, body, default);
        }
    }
}