using Application.Interfaces;
using Domain;
using Domain.Events;

namespace Application
{
    public sealed class BroadcasterService : IBroadcasterService
    {
        private readonly ISettings _settings;
        private readonly IEventBus _eventBus;
        private Timer _timer;

        public BroadcasterService(ISettings settings, IEventBus eventBus)
        {
            _settings = settings;
            _eventBus = eventBus;
        }

        public void StartBroadcasting()
        {
            _timer = new Timer(BroadcastAsync, null, 0, _settings.BroadcastFrequencyMilliSecs);
        }

        private async void BroadcastAsync(object? state)
        {
            var position = Position.CreatePosition();

            await _eventBus.PublishAsync(new PositionCreatedEvent(position.Id, position.Latitude, position.Longitude,
                position.Height));
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
