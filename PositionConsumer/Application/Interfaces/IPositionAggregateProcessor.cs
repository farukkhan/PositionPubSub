namespace Application.Interfaces
{
    public interface IPositionAggregateProcessor : IDisposable
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}