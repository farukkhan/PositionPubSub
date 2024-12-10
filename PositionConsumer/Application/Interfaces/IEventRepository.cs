using IntergrationEvents;

namespace Application.Interfaces
{
    public interface IEventRepository
    {
        void PersistEvent(PositionCreatedIntegrationEvent positionCreatedEvent);
        IEnumerable<PositionCreatedIntegrationEvent> GetEventsAndDequeue(int count);
    }
}