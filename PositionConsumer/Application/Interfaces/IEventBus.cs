using Domain.Events;

namespace Application.Interfaces
{
    public interface IEventBus
    {
        Task ReceiveAsync();
    }
}