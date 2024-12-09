using Domain.Entities;
using Domain.Events;

namespace Domain.AggregateSpecifications
{
    internal interface IAggregateSpecification
    {
        bool IsTrue(AggregatedPosition aggregatedPosition, PositionCreatedEvent position);
    }
}
