namespace Application.Interfaces
{
    public interface IEventBus
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}