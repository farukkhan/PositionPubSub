namespace IntergrationEvents
{
    public class PositionCreatedIntegrationEvent : IntegrationEventBase
    {
        public Guid Id{ get; }
        public double Latitude{ get; }
        public double Longitude{ get; }
        public double Height{ get; }
        public DateTime CreateDateTime{ get; }

        public PositionCreatedIntegrationEvent(Guid id, double latitude, double longitude, double height,
            DateTime createDateTime)
        {
            Id = id;
            Latitude = latitude;
            Longitude = longitude;
            Height = height;
            CreateDateTime = createDateTime;
        }
    }
}
