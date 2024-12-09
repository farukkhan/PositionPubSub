namespace Domain.Events
{
    public class PositionCreatedEvent
    {
        public required Guid Id{ get; init; }
        public required double Latitude{ get; init; }
        public required double Longitude{ get; init; }
        public required double Height{ get; init; }
        public required DateTime CreateDateTime{ get; init; }
    }
}