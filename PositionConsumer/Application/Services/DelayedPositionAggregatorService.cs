using Application.Interfaces;
using IntergrationEvents;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class DelayedPositionAggregatorService(
        IAggregatedPositionRepository aggregatedPositionRepository,
        IAggregatorServiceHelper aggregateServiceHelper,
        ILogger<PositionAggregatorService> logger)
        : IDelayedPositionAggregatorService
    {
        public void Aggregate(
            IEnumerable<PositionCreatedIntegrationEvent> events)
        {
            //Maybe the delayed positions are from discrete series
            var createDateTimes =
                events.Select(evt => evt.CreateDateTime).ToList().Distinct();

            var recentAggregatedPositions = aggregatedPositionRepository.GetByDateTimes(createDateTimes).ToList();

            foreach (var @event in events.Distinct())
            {
                try
                {
                    var existingAggregatedPosition =
                        aggregateServiceHelper.GetAggregatedPositionForCreationTime(
                            recentAggregatedPositions,
                            @event.CreateDateTime);

                    if (existingAggregatedPosition != null)
                    {
                        if (aggregateServiceHelper.AverageWithAggregatedPosition(existingAggregatedPosition,
                                @event))
                        {
                            //Recalculation event can also be fired and from the handler message can be shown to user.
                            logger.LogInformation(
                                "Aggregated position with Id:{0} has been recalculated.",
                                existingAggregatedPosition.Id);
                        }
                    }
                    else
                    {
                        var aggregatedPosition =
                            aggregateServiceHelper.CreateAggregatedPosition(@event);
                        aggregatedPositionRepository.Add(aggregatedPosition);

                        recentAggregatedPositions.Add(aggregatedPosition);
                    }
                }
                catch (Exception ex) //If one fails should not affect the rest
                {
                    logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}
