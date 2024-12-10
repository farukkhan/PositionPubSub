using System.Reflection;
using Application;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Application.Processes;
using Infrastructure.Repository;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Presentation
{
    public static class DependencyContainer
    {
        public static ServiceProvider Register()
        {
            IConfigurationRoot configBuilder = new ConfigurationBuilder()
                                               .AddJsonFile("appsettings.json")
                                               .Build();

            var serviceProvider = new ServiceCollection()
                                  .AddSingleton<IPositionAggregateProcessor, PositionAggregatorProcess>()
                                  .AddSingleton<IPositionAggregatorService, PositionAggregatorService>()
                                  .AddTransient<IPositionConsumerProcess, PositionConsumerProcess>()
                                  .AddTransient<IEventBus, MassTransitEventBus>()
                                  .AddSingleton<IEventRepository, EventRepository>()
                                  .AddSingleton<IAggregatedPositionRepository, AggregatedPositionRepository>()
                                  .Configure<RabbitMQSettings>(configBuilder.GetRequiredSection("RabbitMQSettings"))
                                  .AddLogging(builder =>
                                  {
                                      builder.ClearProviders();
                                      builder.AddConsole();
                                      builder.SetMinimumLevel(LogLevel.Information);
                                  })
                                  .AddMassTransit(config =>
                                  {
                                      config.AddConsumers(Assembly.GetAssembly(typeof(MassTransitEventBus)));

                                      config.UsingRabbitMq((context, configure) =>
                                      {
                                          var settings = configBuilder.GetSection("RabbitMQSettings")
                                                                      .Get<RabbitMQSettings>();

                                          configure.Host(new Uri(settings.Host),
                                              host =>
                                              {
                                                  host.Username(settings.UserName);
                                                  host.Password(settings.Password);
                                              });
                                          configure.ConfigureEndpoints(context);
                                      });
                                  })
                                  .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
