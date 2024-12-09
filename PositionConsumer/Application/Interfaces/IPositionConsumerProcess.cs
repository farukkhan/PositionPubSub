namespace Application.Interfaces
{
    public interface IPositionConsumerProcess
    {
        Task StartAsync(CancellationToken cancellationToken = default);
    }
}