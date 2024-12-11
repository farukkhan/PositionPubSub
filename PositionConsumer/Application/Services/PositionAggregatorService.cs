using Microsoft.Extensions.Logging;
using Application.Interfaces;
using IntergrationEvents;

namespace Application.Services
{
    public class PositionAggregatorService(
        IAggregatedPositionRepository aggregatedPositionRepository,
        IDelayedPositionAggregatorService delayedPositionAggregatorService,
        IAggregatorServiceHelper aggregateServiceHelper,
        ILogger<PositionAggregatorService> logger)
        : IPositionAggregatorService
    {
        public void Aggregate(IEnumerable<PositionCreatedIntegrationEvent> events)
        {
            var delayedEvents = new List<PositionCreatedIntegrationEvent>();
            var recentAggregatedPositions =
                aggregatedPositionRepository.GetLastFive().OrderBy(p => p.NewestTime).ToList();

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
                        aggregateServiceHelper.AverageWithAggregatedPosition(existingAggregatedPosition,
                            @event);
                    }
                    else if (aggregateServiceHelper.IsDelayedEvent(recentAggregatedPositions,
                                 @event.CreateDateTime)) //DelayedPositionEvent
                    {
                        delayedEvents.Add(@event);
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

            delayedPositionAggregatorService.Aggregate(delayedEvents);

            aggregatedPositionRepository.Save();
        }
    }
}