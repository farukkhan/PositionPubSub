namespace Domain
{
    public class Position
    {
        public Guid Id{ get; }
        public double Latitude{ get; }
        public double Longitude{ get; }
        public double Height{ get; }
        public DateTime CreateDateTime{ get; }

        private Position(double latitude, double longitude, double height)
        {
            Id = Guid.NewGuid();
            Latitude = latitude;
            Longitude = longitude;
            Height = height;
            CreateDateTime = DateTime.UtcNow;
        }

        public static Position CreatePosition()
        {
            double minLat = NetherlandsCoordinateRange.MinLatitude;
            double minLon = NetherlandsCoordinateRange.MinLongitude;
            double maxLat = NetherlandsCoordinateRange.MaxLatitude;
            double maxLon = NetherlandsCoordinateRange.MaxLongitude;

            Random r = new Random();

            var latitude = r.NextDouble() * (maxLat - minLat) + minLat;
            var longitude = r.NextDouble() * (maxLon - minLon) + minLon;

            return new Position(latitude, longitude, 0);
        }
    }
}
