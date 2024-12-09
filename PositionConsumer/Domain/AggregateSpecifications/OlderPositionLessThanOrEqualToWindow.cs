using Domain.Entities;
using Domain.Events;

namespace Domain.AggregateSpecifications
{
    internal class OlderPositionLessThanOrEqualToWindow : IAggregateSpecification
    {
        public bool IsTrue(AggregatedPosition aggregatedPosition, PositionCreatedEvent position)
        {
            if (aggregatedPosition.OldestTime.HasValue)
            {
                if (aggregatedPosition.OldestTime.Value <= position.CreateDateTime &&
                    (position.CreateDateTime - aggregatedPosition.NewestTime).TotalSeconds <= 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
