using Application.Interfaces;
using Domain.Entities;
using IntergrationEvents;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Services
{
    public class AggregatorServiceHelper(ILogger<AggregatorServiceHelper> logger) : IAggregatorServiceHelper
    {
        public AggregatedPosition CreateAggregatedPosition(PositionCreatedIntegrationEvent @event)
        {
            var aggregatedPosition = new AggregatedPosition(@event.Latitude,
                @event.Longitude,
                @event.Height,
                @event.CreateDateTime, null);

            LogAggregatedPosition(aggregatedPosition);

            return aggregatedPosition;
        }

        public bool AverageWithAggregatedPosition(AggregatedPosition aggregatedPosition,
            PositionCreatedIntegrationEvent @event)
        {
            if (!aggregatedPosition.DoAverage(@event.Latitude,
                    @event.Longitude,
                    @event.Height, @event.CreateDateTime))
            {
                logger.LogError(
                    "Average failed for the Event:{0} and Aggregate: {1}", @event.Id,
                    aggregatedPosition.Id);

                return false;
            }

            LogAggregatedPosition(aggregatedPosition);

            return true;
        }

        public AggregatedPosition? GetAggregatedPositionForCreationTime(
            IEnumerable<AggregatedPosition> aggregatedPositions,
            DateTime positionCreatedTime)
        {
            var existingAggregatedPosition = aggregatedPositions.FirstOrDefault(ap =>
                ap.CanBeAggregated(positionCreatedTime));

            return existingAggregatedPosition;
        }

        public bool IsDelayedEvent(IEnumerable<AggregatedPosition> aggregatedPositions,
            DateTime eventCreateDateTime)
        {
            return aggregatedPositions.Any() && eventCreateDateTime <
                aggregatedPositions.Min(p => p.NewestTime);
        }

        private void LogAggregatedPosition(AggregatedPosition previousAggregatedPosition)
        {
            var jsonAggregatedPosition = JsonConvert.SerializeObject(previousAggregatedPosition);

            logger.LogInformation("Aggregated : {0}", jsonAggregatedPosition);
        }
    }
}