using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Application.Interfaces;

namespace Application.Processes
{
    public class PositionAggregatorProcess(
        IPositionAggregatorService positionAggregatorService,
        IEventRepository positionRepository,
        ILogger<PositionAggregatorProcess> logger)
        : BackgroundService, IPositionAggregatorProcess
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("PositionAggregatorProcess running.");

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    try
                    {
                        var positionsToAggregate = positionRepository.GetEventsAndDequeue(500);
                        positionAggregatorService.Aggregate(positionsToAggregate);
                    }
                    catch (Exception
                           ex) //Running in the background exception handling is very important otherwise will fail silently
                    {
                        logger.LogError(ex, "Error during aggregate.");
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                logger.LogError(ex, "Error in aggregate process.");
            }
        }
    }
}