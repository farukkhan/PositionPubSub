using Application.Interfaces;
using Domain.Events;

namespace Infrastructure.Repository
{
    public class PositionRepository : IEventRepository
    {
        private readonly List<PositionCreatedEvent> _positions = new();
        private readonly object _lock = new();

        public void PersistEvent(PositionCreatedEvent positionCreatedEvent)
        {
            lock (_lock)
            {
                _positions.Add(positionCreatedEvent);
            }
        }

        public IEnumerable<PositionCreatedEvent> GetEvents(int count)
        {
            List<PositionCreatedEvent> positionsToProcess;

            lock (_lock)
            {
                positionsToProcess = _positions.Take(count).ToList();
                _positions.RemoveRange(0, _positions.Count < count ? _positions.Count : count);
            }

            return positionsToProcess;
        }
    }
}
