using Application.Interfaces;
using Domain;
using Domain.Events;
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
            var position = Position.CreatePosition();

            await eventBus.PublishAsync(new PositionCreatedEvent(position.Id, position.Latitude, position.Longitude,
                position.Height));
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
