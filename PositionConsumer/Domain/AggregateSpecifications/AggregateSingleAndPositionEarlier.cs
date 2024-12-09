using Domain.Entities;
using Domain.Events;

namespace Domain.AggregateSpecifications
{
    public class AggregateSingleAndPositionEarlier : IAggregateSpecification
    {
        public bool IsTrue(AggregatedPosition aggregatedPosition, PositionCreatedEvent position)
        {
            if (!aggregatedPosition.OldestTime.HasValue)
            {
                if (Math.Abs((aggregatedPosition.NewestTime - position.CreateDateTime).TotalSeconds) <= 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
