namespace Domain.Events
{
    public class PositionCreatedEvent : EventBase
    {
        public Guid Id{ get; }
        public double Latitude{ get; }
        public double Longitude{ get; }
        public double Height{ get; }

        public PositionCreatedEvent(Guid id, double latitude, double longitude, double height)
        {
            Id = id;
            Latitude = latitude;
            Longitude = longitude;
            Height = height;
        }
    }
}
