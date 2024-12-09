using Domain.Events;

namespace Application.Interfaces
{
    public interface IEventRepository
    {
        void PersistEvent(PositionCreatedEvent positionCreatedEvent);
        IEnumerable<PositionCreatedEvent> GetEvents(int count);
    }
}