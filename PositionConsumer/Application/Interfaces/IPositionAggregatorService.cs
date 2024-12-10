using IntergrationEvents;

namespace Application.Interfaces
{
    public interface IPositionAggregatorService
    {
        void Aggregate(IEnumerable<PositionCreatedIntegrationEvent> positionsToAggregate);
    }
}