using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Application.Interfaces;

namespace Application.Processes
{
    public class PositionAggregatorProcess(
        IPositionAggregatorService positionAggregatorService,
        IEventRepository positionRepository,
        ILogger<PositionAggregatorProcess> logger)
        : BackgroundService, IPositionAggregateProcessor
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("PositionAggregatorProcess running.");

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    var positionsToAggregate = positionRepository.GetEvents(500);
                    positionAggregatorService.Aggregate(positionsToAggregate);
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("PositionAggregatorProcess is stopping.");
            }
        }
    }
}