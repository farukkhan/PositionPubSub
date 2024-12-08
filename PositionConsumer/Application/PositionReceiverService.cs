using Application.Interfaces;

namespace Application
{
    public class PositionReceiverService : IPositionReceiverService
    {
        private readonly IEventBus _eventBus;

        public PositionReceiverService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _eventBus.ReceiveAsync(cancellationToken: cancellationToken);
        }
    }
}
