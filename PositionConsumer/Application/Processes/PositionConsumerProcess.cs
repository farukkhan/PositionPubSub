using Application.Interfaces;

namespace Application.Processes
{
    public class PositionConsumerProcess(
        IEventBus eventBus,
        IPositionAggregatorProcess positionAggregatorService)
        : IPositionConsumerProcess
    {
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            var eventbusTask = eventBus.StartAsync(cancellationToken: cancellationToken);
            var aggregatorStartTask = positionAggregatorService.StartAsync(cancellationToken);

            return Task.WhenAll(eventbusTask, aggregatorStartTask);
        }
    }
}