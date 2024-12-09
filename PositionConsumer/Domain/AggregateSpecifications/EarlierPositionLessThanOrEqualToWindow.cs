using Domain.Entities;
using Domain.Events;

namespace Domain.AggregateSpecifications
{
    public class EarlierPositionLessThanOrEqualToWindow : IAggregateSpecification
    {
        public bool IsTrue(AggregatedPosition aggregatedPosition, PositionCreatedEvent position)
        {
            if (aggregatedPosition.OldestTime.HasValue)
            {
                if (aggregatedPosition.NewestTime >= position.CreateDateTime &&
                    (aggregatedPosition.OldestTime.Value - position.CreateDateTime).TotalSeconds <= 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
