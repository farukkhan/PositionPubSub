using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAggregatedPositionRepository
    {
        void Add(AggregatedPosition positionCreatedEvent);
        IEnumerable<AggregatedPosition> GetByDateTimes(IEnumerable<DateTime> dateTimes);
        IEnumerable<AggregatedPosition> GetLastFive();
        void SaveChanges();
    }
}
