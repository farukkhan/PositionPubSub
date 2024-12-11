namespace Application.Interfaces
{
    public interface IPositionAggregatorProcess : IDisposable
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}