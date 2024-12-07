using Domain.Events;

namespace Application.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync(EventBase @event);
    }
}