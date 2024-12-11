using Microsoft.Extensions.DependencyInjection;
using Application;
using Infrastructure;
using Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
                                  .AddTransient<IBroadcasterService, BroadcasterService>()
                                  .AddTransient<IEventBus, MassTransitEventBus>()
                                  .AddLogging(builder =>
                                  {
                                      builder.ClearProviders();
                                      builder.AddSerilog(logger);
                                      builder.SetMinimumLevel(LogLevel.Information);
                                  })
                                  .Configure<Settings>(configBuilder.GetRequiredSection("Settings"))
                                  .Configure<RabbitMQSettings>(configBuilder.GetRequiredSection("RabbitMQSettings"))
                                  .AddAndConfigureMassTransit(configBuilder)
                                  .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
