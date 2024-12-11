using Microsoft.Extensions.DependencyInjection;
using Infrastructure;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Application.Processes;
using Application.Services;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Presentation
{
    public static class DependencyContainer
    {
        public static ServiceProvider Register()
        {
            IConfigurationRoot configBuilder = new ConfigurationBuilder()
                                               .AddJsonFile("appsettings.json")
                                               .Build();

            var logger = new LoggerConfiguration()
                         .WriteTo.File(@"Logs/Log.txt", rollingInterval: RollingInterval.Minute)
                         .MinimumLevel.Information()
                         .CreateLogger();

            var serviceProvider = new ServiceCollection()
                                  .AddSingleton<IPositionAggregatorProcess, PositionAggregatorProcess>()
                                  .AddSingleton<IPositionAggregatorService, PositionAggregatorService>()
                                  .AddSingleton<IDelayedPositionAggregatorService, DelayedPositionAggregatorService>()
                                  .AddSingleton<IAggregatorServiceHelper, AggregatorServiceHelper>()
                                  .AddTransient<IPositionConsumerProcess, PositionConsumerProcess>()
                                  .AddTransient<IEventBus, MassTransitEventBus>()
                                  .AddSingleton<IEventRepository, EventRepository>()
                                  .AddSingleton<IAggregatedPositionRepository, AggregatedPositionRepository>()
                                  .Configure<RabbitMQSettings>(configBuilder.GetRequiredSection("RabbitMQSettings"))
                                  .AddLogging(builder =>
                                  {
                                      builder.ClearProviders();
                                      builder.AddSerilog(logger);
                                      builder.SetMinimumLevel(LogLevel.Information);
                                  })
                                  .AddAndConfigureMassTransit(configBuilder)                                  
                                  .BuildServiceProvider();

            return serviceProvider;
        }
    }
}