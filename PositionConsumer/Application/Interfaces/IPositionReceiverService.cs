namespace Application.Interfaces
{
    public interface IPositionReceiverService
    {
        Task StartAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
