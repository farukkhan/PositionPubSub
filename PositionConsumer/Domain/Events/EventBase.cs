using MediatR;

namespace Domain.Events
{
    public class EventBase : INotification
    {
        public DateTime Timestamp{ get; protected set; }

        protected EventBase(DateTime time)
        {
            Timestamp = time;
        }
    }
}
