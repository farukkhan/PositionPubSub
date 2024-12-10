using Domain.Events;

namespace Application.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event) where T : EventBase;
    }
}