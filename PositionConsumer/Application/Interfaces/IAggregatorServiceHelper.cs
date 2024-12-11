using Domain.Entities;
using IntergrationEvents;

namespace Application.Interfaces
{
    public interface IAggregatorServiceHelper
    {
        AggregatedPosition CreateAggregatedPosition(
            PositionCreatedIntegrationEvent @event);

        bool AverageWithAggregatedPosition(AggregatedPosition aggregatedPosition,
            PositionCreatedIntegrationEvent @event);

        AggregatedPosition? GetAggregatedPositionForCreationTime(
            IEnumerable<AggregatedPosition> aggregatedPositions,
            DateTime positionCreatedTime);

        bool IsDelayedEvent(IEnumerable<AggregatedPosition> recentAggregatedPositions,
            DateTime eventCreateDateTime);

        //void LogAggregatedPosition(AggregatedPosition previousAggregatedPosition);
    }
}