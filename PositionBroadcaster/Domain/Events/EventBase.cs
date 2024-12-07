namespace Domain.Events
{
    public class EventBase
    {
        public DateTime Timestamp{ get; protected set; }

        protected EventBase()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
