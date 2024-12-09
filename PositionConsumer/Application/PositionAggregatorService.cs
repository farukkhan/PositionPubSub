using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Domain.Events;
using Domain.Entities;

namespace Application
{
    public class PositionAggregatorService(
        IAggregatedPositionRepository aggregatedPositionRepository,
        ILogger<PositionAggregatorService> logger)
        : IPositionAggregatorService
    {
        public void Aggregate(IEnumerable<PositionCreatedEvent> positionsToAggregate)
        {
            foreach (var positionCreatedEvent in positionsToAggregate.ToList().Distinct())
            {
                var lastAggregatedPosition = aggregatedPositionRepository.AggregatedPositions
                                                                         ?.Find(ap =>
                                                                             ap.CanBeAggregated(positionCreatedEvent
                                                                                 .CreateDateTime));
                if (lastAggregatedPosition != null)
                {
                    if (!lastAggregatedPosition.DoAverage(positionCreatedEvent.Latitude, positionCreatedEvent.Longitude,
                            positionCreatedEvent.Height, positionCreatedEvent.CreateDateTime))
                    {
                        logger.LogError(
                            $"Average failed for the Event:{positionCreatedEvent.Id} and Aggregate: {lastAggregatedPosition.Id}");
                    }
                }
                else
                {
                    lastAggregatedPosition = new AggregatedPosition(positionCreatedEvent.Latitude,
                        positionCreatedEvent.Longitude,
                        positionCreatedEvent.Height,
                        positionCreatedEvent.CreateDateTime, null);

                    aggregatedPositionRepository.AggregatedPositions?.Add(lastAggregatedPosition);
                }

                logger.LogInformation(
                    $"Aggregated Position :{lastAggregatedPosition.Latitude},{lastAggregatedPosition.Longitude},{lastAggregatedPosition.Height},{lastAggregatedPosition.NewestTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt")},{(lastAggregatedPosition.OldestTime.HasValue ? lastAggregatedPosition.OldestTime.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt") : lastAggregatedPosition.OldestTime)}");
            }
        }

        //private static bool FindPosition(AggregatedPosition aggregatedPosition, PositionCreatedEvent position)
        //{
        //    if (!aggregatedPosition.OldestTime.HasValue)
        //    {
        //        if (Math.Abs((aggregatedPosition.NewestTime - position.Timestamp).TotalSeconds) <= 1)
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        if (aggregatedPosition.NewestTime >= position.Timestamp &&
        //            (aggregatedPosition.OldestTime.Value - position.Timestamp).TotalSeconds <= 1)
        //        {
        //            return true;
        //        }


        //        if (aggregatedPosition.OldestTime.Value <= position.Timestamp &&
        //            (position.Timestamp - aggregatedPosition.NewestTime).TotalSeconds <= 1)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }
}