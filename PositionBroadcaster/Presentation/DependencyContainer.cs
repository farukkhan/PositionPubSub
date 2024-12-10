using Microsoft.Extensions.DependencyInjection;
using Application;
using Infrastructure;
using Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
                                  .AddTransient<IBroadcasterService, BroadcasterService>()
                                  .AddTransient<IEventBus, MassTransitEventBus>()
                                  .AddLogging(builder =>
                                  {
                                      builder.ClearProviders();
                                      builder.AddConsole();
                                      builder.SetMinimumLevel(LogLevel.Warning);
                                  })
                                  .Configure<Settings>(configBuilder.GetRequiredSection("Settings"))
                                  .Configure<RabbitMQSettings>(configBuilder.GetRequiredSection("RabbitMQSettings"))
                                  .AddMassTransit(config =>
                                  {
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
