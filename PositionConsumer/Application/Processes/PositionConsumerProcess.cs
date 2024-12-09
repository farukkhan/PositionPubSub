using Application.Interfaces;

namespace Application.Processes
{
    public class PositionConsumerProcess(IEventBus eventBus, IPositionAggregateProcessor positionAggregatorService)
        : IPositionConsumerProcess
    {
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            var receiveTask = eventBus.ReceiveAsync(cancellationToken: cancellationToken);
            var aggregatorStartTask = positionAggregatorService.StartAsync(cancellationToken);

            return Task.WhenAll(receiveTask, aggregatorStartTask);
        }
    }
}