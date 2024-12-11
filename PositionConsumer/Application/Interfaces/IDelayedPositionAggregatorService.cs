using IntergrationEvents;

namespace Application.Interfaces
{
    public interface IDelayedPositionAggregatorService
    {
        void Aggregate(IEnumerable<PositionCreatedIntegrationEvent> positionCreatedIntegrationEvents);
    }
}