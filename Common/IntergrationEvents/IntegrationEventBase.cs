namespace IntergrationEvents
{
    public class IntegrationEventBase
    {
        public DateTime CreateDateTime{ get; protected set; }

        protected IntegrationEventBase()
        {
            CreateDateTime = DateTime.UtcNow;
        }
    }
}