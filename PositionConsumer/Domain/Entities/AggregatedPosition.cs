using Domain.Events;

namespace Domain.Entities
{
    public class AggregatedPosition
    {
        public Guid Id{ get; }
        public double Latitude{ get; private set; }
        public double Longitude{ get; private set; }
        public double Height{ get; private set; }
        public DateTime NewestTime{ get; private set; }
        public DateTime? OldestTime{ get; private set; }

        public AggregatedPosition(double latitude, double longitude, double height, DateTime newestDateTime,
            DateTime? oldestDateTime)
        {
            Id = Guid.NewGuid();
            Latitude = latitude;
            Longitude = longitude;
            Height = height;
            NewestTime = newestDateTime;
            OldestTime = oldestDateTime;
        }

        public bool DoAverage(double latitude, double longitude, double height, DateTime positionCreateDateTime)
        {
            if (CanBeAggregated(positionCreateDateTime))
            {
                Latitude = (Latitude + latitude) / 2;
                Longitude = (Longitude + longitude) / 2;
                Height = (Height + height) / 2;
                NewestTime = NewestTime < positionCreateDateTime
                    ? NewestTime
                    : positionCreateDateTime;
                OldestTime = OldestTime.HasValue &&
                    OldestTime.Value > positionCreateDateTime
                        ? OldestTime
                        : positionCreateDateTime;

                return true;
            }

            return false;
        }

        public bool CanBeAggregated(DateTime createdTime)
        {
            if (!OldestTime.HasValue)
            {
                if (Math.Abs((NewestTime - createdTime).TotalSeconds) <= 1)
                {
                    return true;
                }
            }
            else
            {
                if (NewestTime >= createdTime &&
                    (OldestTime.Value - createdTime).TotalSeconds <= 1)
                {
                    return true;
                }


                if (OldestTime.Value <= createdTime &&
                    (createdTime - NewestTime).TotalSeconds <= 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
