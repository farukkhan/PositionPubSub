using Application.Interfaces;
using MassTransit;

namespace Infrastructure
{
    public class MassTransitEventBus(IBusControl busControl) : IEventBus
    {
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return busControl.StartAsync(new CancellationToken());
        }
    }
}
