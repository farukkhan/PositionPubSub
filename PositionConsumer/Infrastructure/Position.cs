namespace Infrastructure
{
    public class Position
    {
        public Guid Id{ get; set; }
        public double Latitude{ get; set; }
        public double Longitude{ get; set; }
        public double Height{ get; set; }
        public DateTime Timestamp{ get; set; }
    }
}
