using Microsoft.Extensions.DependencyInjection;
using Application;
using Infrastructure;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Presentation
{
    public static class DependencyContainer
    {
        public static ServiceProvider Register()
        {
            var serviceProvider = new ServiceCollection()
                                  .AddTransient<IBroadcasterService, BroadcasterService>()
                                  .AddSingleton<ISettings, Settings>()
                                  .AddTransient<IEventBus, RabbitMQEventBus>()
                                  .AddLogging(builder =>
                                  {
                                      builder.ClearProviders();
                                      builder.AddConsole();
                                      builder.SetMinimumLevel(LogLevel.Warning);
                                  })
                                  .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
