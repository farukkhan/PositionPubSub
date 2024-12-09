using Application;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Application.Processes;
using Infrastructure.Repository;

namespace Presentation
{
    public static class DependencyContainer
    {
        public static ServiceProvider Register()
        {
            var serviceProvider = new ServiceCollection()
                                  .AddSingleton<IPositionAggregateProcessor, PositionAggregatorProcess>()
                                  .AddSingleton<IPositionAggregatorService, PositionAggregatorService>()
                                  .AddTransient<IPositionConsumerProcess, PositionConsumerProcess>()
                                  .AddTransient<IEventBus, RabbitMQEventBus>()
                                  .AddSingleton<IEventRepository, PositionRepository>()
                                  .AddSingleton<IAggregatedPositionRepository, AggregatedPositionRepository>()
                                  .AddLogging(builder =>
                                  {
                                      builder.ClearProviders();
                                      builder.AddConsole();
                                      builder.SetMinimumLevel(LogLevel.Information);
                                  })
                                  .BuildServiceProvider();
            return serviceProvider;
        }
    }
}
