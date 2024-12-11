using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using IntergrationEvents;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Test
{
    [TestClass]
    public class DelayedPositionAggregatorServiceTest
    {
        private readonly Mock<IAggregatedPositionRepository> _aggregatedPositionRepositoryMock = new();
        private readonly Mock<IAggregatorServiceHelper> _aggregateServiceHelperMock = new();
        private readonly Mock<ILogger<PositionAggregatorService>> _loggerMock = new();

        private DelayedPositionAggregatorService _delayedPositionAggregatorService;

        [TestInitialize]
        public void Initialize()
        {
            _delayedPositionAggregatorService = new DelayedPositionAggregatorService(
                _aggregatedPositionRepositoryMock.Object,
                _aggregateServiceHelperMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public void Aggregate_GivenPositionEventsAreContinuationFromPreviouslyAggregated_DoAverage()
        {
            var events = SetUpEvents();
            var previouslyAggregatedPositions = SetupAggregatedPositions();
            var existingAggregatedPosition = previouslyAggregatedPositions.First();

            _aggregatedPositionRepositoryMock.Setup(x => x.GetLastFive()).Returns(previouslyAggregatedPositions);
            _aggregateServiceHelperMock
                .Setup(x => x.GetAggregatedPositionForCreationTime(It.IsAny<IEnumerable<AggregatedPosition>>(),
                    It.IsAny<DateTime>()))
                .Returns(existingAggregatedPosition);

            _delayedPositionAggregatorService.Aggregate(events);

            _aggregateServiceHelperMock.Verify(
                x => x.AverageWithAggregatedPosition(It.IsAny<AggregatedPosition>(),
                    It.IsAny<PositionCreatedIntegrationEvent>()),
                Times.Exactly(5));
        }

        [TestMethod]
        public void Aggregate_GivenPositionEventsAreNotContinuationExistingAggregates_AddNewAggregate()
        {
            //Arrange
            var events = SetUpEvents();
            var recentAggregatedPositions = SetupAggregatedPositions();

            _aggregatedPositionRepositoryMock.Setup(x => x.GetLastFive()).Returns(recentAggregatedPositions);

            //Act
            _delayedPositionAggregatorService.Aggregate(events);

            //Assert
            _aggregatedPositionRepositoryMock.Verify(x => x.Add(It.IsAny<AggregatedPosition>()), Times.Exactly(5));
        }

        private List<AggregatedPosition> SetupAggregatedPositions()
        {
            List<AggregatedPosition> aggregatedPositions = new List<AggregatedPosition>();

            aggregatedPositions.Add(new AggregatedPosition(51.57, 6.83, 4.09, DateTime.UtcNow.AddMilliseconds(-20000),
                DateTime.UtcNow.AddMilliseconds(-19500)));
            aggregatedPositions.Add(new AggregatedPosition(52.88, 7.83, 5.11, DateTime.UtcNow.AddMilliseconds(-21000),
                DateTime.UtcNow.AddMilliseconds(-20500)));
            aggregatedPositions.Add(new AggregatedPosition(53.67, 8.83, 3.88, DateTime.UtcNow.AddMilliseconds(-18000),
                DateTime.UtcNow.AddMilliseconds(-17500)));
            aggregatedPositions.Add(new AggregatedPosition(56.55, 4.83, 2.55, DateTime.UtcNow.AddMilliseconds(-16000),
                DateTime.UtcNow.AddMilliseconds(-15500)));
            aggregatedPositions.Add(new AggregatedPosition(47.57, 5.83, 7.49, DateTime.UtcNow.AddMilliseconds(-12000),
                DateTime.UtcNow.AddMilliseconds(-11500)));

            return aggregatedPositions;
        }

        private List<PositionCreatedIntegrationEvent> SetUpEvents()
        {
            List<PositionCreatedIntegrationEvent> events = new List<PositionCreatedIntegrationEvent>();

            events.Add(new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-20000)));
            events.Add(new PositionCreatedIntegrationEvent(Guid.NewGuid(), 52.88, 7.83, 5.11,
                DateTime.UtcNow.AddMilliseconds(-21000)));
            events.Add(new PositionCreatedIntegrationEvent(Guid.NewGuid(), 53.67, 8.83, 3.88,
                DateTime.UtcNow.AddMilliseconds(-18000)));
            events.Add(new PositionCreatedIntegrationEvent(Guid.NewGuid(), 56.55, 4.83, 2.55,
                DateTime.UtcNow.AddMilliseconds(-16000)));
            events.Add(new PositionCreatedIntegrationEvent(Guid.NewGuid(), 47.57, 5.83, 7.49,
                DateTime.UtcNow.AddMilliseconds(-12000)));

            return events;
        }
    }
}
