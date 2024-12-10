using Application.Interfaces;
using IntergrationEvents;

namespace Infrastructure.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly List<PositionCreatedIntegrationEvent> _positions = new();
        private readonly object _lock = new();

        public void PersistEvent(PositionCreatedIntegrationEvent positionCreatedEvent)
        {
            lock (_lock)
            {
                _positions.Add(positionCreatedEvent);
            }
        }

        public IEnumerable<PositionCreatedIntegrationEvent> GetEventsAndDequeue(int count)
        {
            List<PositionCreatedIntegrationEvent> positionsToProcess;

            lock (_lock)
            {
                positionsToProcess = _positions.Take(count).ToList();
                _positions.RemoveRange(0, _positions.Count < count ? _positions.Count : count);
            }

            return positionsToProcess;
        }
    }
}