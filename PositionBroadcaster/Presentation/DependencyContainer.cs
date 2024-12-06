using Microsoft.Extensions.DependencyInjection;
using Application;

namespace Presentation
{
    internal class DependencyContainer
    {
        public ServiceProvider Register()
        {
            var serviceProvider = new ServiceCollection()
                                  .AddTransient<IBroadcasterService, BroadcasterService>()
                                  .AddSingleton<ISettings, Settings>()
                                  .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
