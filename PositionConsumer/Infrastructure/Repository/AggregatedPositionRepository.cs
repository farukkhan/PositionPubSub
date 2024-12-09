using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repository
{
    public class AggregatedPositionRepository : IAggregatedPositionRepository
    {
        public List<AggregatedPosition> AggregatedPositions{ get; } = new();
    }
}
