using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repository
{
    public class AggregatedPositionRepository : IAggregatedPositionRepository
    {
        private readonly List<AggregatedPosition> _aggregatedPositions = new();
        private readonly List<AggregatedPosition> _lastFiveAggregatedPositions = new();

        private readonly object _lock = new();

        public void Add(AggregatedPosition positionCreatedEvent)
        {
            lock (_lock)
            {
                _aggregatedPositions.Add(positionCreatedEvent);
            }
        }

        /// <summary>
        /// Considering <paramref name="dateTimes"/> contains discrete series of positions.
        /// To make it for performant, can be grouped the dateTimes based on the series
        /// </summary>
        /// <param name="dateTimes"></param>
        /// <returns></returns>
        public IEnumerable<AggregatedPosition> GetByDateTimes(IEnumerable<DateTime> dateTimes)
        {
            List<AggregatedPosition> aggregatedPositions = new List<AggregatedPosition>();

            foreach (var dateTime in dateTimes)
            {
                aggregatedPositions.AddRange(_aggregatedPositions.Where(ap =>
                    dateTime < ap.NewestTime.AddSeconds(1) && dateTime > ap.NewestTime.AddSeconds(-1)).ToList());
            }


            //Note : In real database, expression can be created with OR Where

            //IQueryable<AggregatedPosition> arreAggregatedPositions = _aggregatedPositions.AsQueryable();
            //foreach (var dateTime in dateTimes)
            //{
            //    arreAggregatedPositions = arreAggregatedPositions.Where(ap =>
            //        dateTime < ap.NewestTime.AddSeconds(1) && dateTime > ap.NewestTime.AddSeconds(-1));
            //}

            return aggregatedPositions.Distinct().ToList();
        }

        public IEnumerable<AggregatedPosition> GetLastFive()
        {
            return _lastFiveAggregatedPositions;
        }

        public void SaveChanges()
        {
            //Note : Nothing to save, as the repository is in memory
            _lastFiveAggregatedPositions.Clear();
            _lastFiveAggregatedPositions.AddRange(_aggregatedPositions.OrderBy(p => p.NewestTime).TakeLast(5));
        }
    }
}