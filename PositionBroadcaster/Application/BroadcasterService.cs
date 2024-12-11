using Application.Interfaces;
using Domain;
using IntergrationEvents;
using Microsoft.Extensions.Options;

namespace Application
{
    public sealed class BroadcasterService(IOptionsMonitor<Settings> settings, IEventBus eventBus) : IBroadcasterService
    {
        private readonly Settings _settings = settings.CurrentValue;
        private Timer _timer;

        public void StartBroadcasting()
        {
            _timer = new Timer(BroadcastAsync, null, 0, _settings.BroadcastFrequencyMilliSecs);
        }

        private async void BroadcastAsync(object? state)
        {
            //Should, PositionCreatedEvent returned. And with event handler raise PositionCreatedIntegrationEvent???
            var position = Position.CreatePosition();

            await eventBus.PublishAsync(new PositionCreatedIntegrationEvent(position.Id, position.Latitude,
                position.Longitude,
                position.Height, position.CreateDateTime));
        }

        public void Dispose()
        {
            //Will be disposed by the IoC
            _timer?.Dispose();
        }
    }
}
