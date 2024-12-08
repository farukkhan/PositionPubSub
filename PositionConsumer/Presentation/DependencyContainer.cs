using Application;
using Microsoft.Extensions.DependencyInjection;
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
                                  .AddTransient<IPositionReceiverService, PositionReceiverService>()
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
