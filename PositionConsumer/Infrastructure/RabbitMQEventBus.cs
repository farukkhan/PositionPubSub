using System.Net.Sockets;
using System.Text;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Infrastructure
{
    public sealed class RabbitMQEventBus : IEventBus
    {
        private readonly ILogger _logger;

        public RabbitMQEventBus(ILogger<RabbitMQEventBus> logger)
        {
            _logger = logger;
        }

        public async Task ReceiveAsync(CancellationToken cancellationToken)
        {
            try
            {
                var pipeline = CreateResiliencePipeline(2);
                await pipeline.ExecuteAsync(async (token) => {await DoReceiveAsync(token);}, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    @"Connection with the position source is broken. Contact with the administrator and restart the application.");
            }
        }

        private async Task DoReceiveAsync(CancellationToken cancellationToken)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                await using var connection = await factory.CreateConnectionAsync(cancellationToken);
                await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

                await channel.QueueDeclareAsync("PositionCreatedEvent", true, false, false, null,
                    cancellationToken: cancellationToken);

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
                    consumer: consumer, cancellationToken);

                // ReSharper disable once EmptyEmbeddedStatement
                while (connection.IsOpen && !cancellationToken.IsCancellationRequested) ;
            }
            catch (Exception e)
            {
                _logger.LogError(message: e.Message);
                throw;
            }
        }

        private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
        {
            var retryOptions = new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
                MaxRetryAttempts = retryCount,
                DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
            };

            return new ResiliencePipelineBuilder()
                   .AddRetry(retryOptions)
                   .Build();
        }

        private static TimeSpan? GenerateDelay(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }
}