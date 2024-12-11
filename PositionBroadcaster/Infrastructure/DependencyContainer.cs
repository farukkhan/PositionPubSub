using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddAndConfigureMassTransit(this IServiceCollection serviceCollection,
            IConfigurationRoot configurationRoot)
        {
            serviceCollection.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, configure) =>
                {
                    var settings = configurationRoot.GetSection("RabbitMQSettings")
                                                    .Get<RabbitMQSettings>();

                    configure.Host(new Uri(settings.Host),
                        host =>
                        {
                            host.Username(settings.UserName);
                            host.Password(settings.Password);
                        });
                    configure.ConfigureEndpoints(context);
                });
            });

            return serviceCollection;
        }
    }
}
