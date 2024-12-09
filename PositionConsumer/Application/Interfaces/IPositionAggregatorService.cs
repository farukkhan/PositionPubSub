using Domain.Events;

namespace Application.Interfaces
{
    public interface IPositionAggregatorService
    {
        void Aggregate(IEnumerable<PositionCreatedEvent> positionsToAggregate);
    }
}