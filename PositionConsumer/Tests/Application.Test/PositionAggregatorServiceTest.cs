﻿using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using IntergrationEvents;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Test
{
    [TestClass]
    public class PositionAggregatorServiceTest
    {
        private Mock<IAggregatedPositionRepository> _aggregatedPositionRepositoryMock = new();
        private Mock<IDelayedPositionAggregatorService> _delayedPositionAggregatorServiceMock = new();
        private Mock<IAggregatorServiceHelper> _aggregateServiceHelperMock = new();
        private Mock<ILogger<PositionAggregatorService>> _loggerMock = new();

        private PositionAggregatorService _positionAggregatorService;

        [TestInitialize]
        public void Initialize()
        {
            _positionAggregatorService = new PositionAggregatorService(_aggregatedPositionRepositoryMock.Object,
                _delayedPositionAggregatorServiceMock.Object, _aggregateServiceHelperMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public void Aggregate_GivenPositionEventsAreContinuationFromRecentlyAggregated_DoAverage()
        {
            var events = SetUpEvents();
            var recentAggregatedPositions = SetUpAggregatedPositions();
            var existingAggretedPosition = recentAggregatedPositions.First();

            _aggregatedPositionRepositoryMock.Setup(x => x.GetLastFive()).Returns(recentAggregatedPositions);
            _aggregateServiceHelperMock
                .Setup(x => x.GetAggregatedPositionForCreationTime(It.IsAny<IEnumerable<AggregatedPosition>>(),
                    It.IsAny<DateTime>()))
                .Returns(existingAggretedPosition);

            _positionAggregatorService.Aggregate(events);

            _aggregateServiceHelperMock.Verify(
                x => x.AverageWithAggregatedPosition(It.IsAny<AggregatedPosition>(),
                    It.IsAny<PositionCreatedIntegrationEvent>()),
                Times.Exactly(5));
        }

        [TestMethod]
        public void Aggregate_GivenPositionEventsAreDelayed_DoDelayedAggregate()
        {
            //Arrange
            var events = SetUpEvents();
            var recentAggregatedPositions = SetUpAggregatedPositions();

            _aggregatedPositionRepositoryMock.Setup(x => x.GetLastFive()).Returns(recentAggregatedPositions);
            _aggregateServiceHelperMock
                .Setup(x => x.IsDelayedEvent(It.IsAny<IEnumerable<AggregatedPosition>>(),
                    It.IsAny<DateTime>()))
                .Returns(true);

            //Act
            _positionAggregatorService.Aggregate(events);

            //Assert
            _aggregateServiceHelperMock.Verify(
                x => x.IsDelayedEvent(It.IsAny<IEnumerable<AggregatedPosition>>(),
                    It.IsAny<DateTime>()),
                Times.Exactly(5));

            _delayedPositionAggregatorServiceMock.Verify(x => x.Aggregate(events), Times.Once);
        }

        [TestMethod]
        public void Aggregate_GivenPositionEventsAreContinuation_ButNotWithinOneSecTimeSpan_AddNewAggregate()
        {
            //Arrange
            var events = SetUpEvents();
            var recentAggregatedPositions = SetUpAggregatedPositions();

            _aggregatedPositionRepositoryMock.Setup(x => x.GetLastFive()).Returns(recentAggregatedPositions);

            //Act
            _positionAggregatorService.Aggregate(events);

            //Assert
            _aggregatedPositionRepositoryMock.Verify(x => x.Add(It.IsAny<AggregatedPosition>()), Times.Exactly(5));
        }

        private List<AggregatedPosition> SetUpAggregatedPositions()
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
