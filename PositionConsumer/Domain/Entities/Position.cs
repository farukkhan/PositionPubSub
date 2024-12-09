namespace Domain.Entities
{
    public record Position //(Guid id, double latitude, double longitude, double height, DateTime createdDateTime)
    {
        public required Guid Id{ get; init; }
        public required double Latitude{ get; init; }
        public required double Longitude{ get; init; }
        public required double Height{ get; init; }
        public required DateTime CreatedDateTime{ get; init; }
    }
}