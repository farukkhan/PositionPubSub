using System.Text;
using Application.Interfaces;
using Domain.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure
{
    public sealed class RabbitMQEventBus : IEventBus
    {
        private readonly ILogger _logger;

        public RabbitMQEventBus(ILogger<RabbitMQEventBus> logger)
        {
            _logger = logger;
        }

        public async Task ReceiveAsync()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                await using var connection = await factory.CreateConnectionAsync();
                await using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync("PositionCreatedEvent", true, false, false, null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var position = JsonConvert.DeserializeObject<Position>(message);
                    Console.WriteLine($"Message received: {position.Id} Timestamp:{position.Timestamp}");
                };

                //read the message
                await channel.BasicConsumeAsync(queue: "PositionCreatedEvent", autoAck: true,
                    consumer: consumer);

                while (connection != null && connection.IsOpen) ;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}