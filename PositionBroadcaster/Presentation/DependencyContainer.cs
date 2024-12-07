using Microsoft.Extensions.DependencyInjection;
using Application;
using Infrastructure;
using Application.Interfaces;

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
                                  .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
