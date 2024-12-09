using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAggregatedPositionRepository
    {
        List<AggregatedPosition> AggregatedPositions{ get; }
    }
}
