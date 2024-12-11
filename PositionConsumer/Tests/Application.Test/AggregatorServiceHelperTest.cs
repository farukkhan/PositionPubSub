using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using IntergrationEvents;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Test
{
    [TestClass]
    public class AggregatorServiceHelperTest
    {
        private readonly Mock<IAggregatedPositionRepository> _aggregatedPositionRepositoryMock = new();
        private readonly Mock<ILogger<AggregatorServiceHelper>> _loggerMock = new();

        private AggregatorServiceHelper _aggregatorServiceHelper;

        [TestInitialize]
        public void Initialize()
        {
            _aggregatorServiceHelper = new AggregatorServiceHelper(_loggerMock.Object);
        }

        [TestMethod]
        public void CreateAggregatedPosition_GivenPositionCreatedEvent_CreateNewAggregateWithLogging()
        {
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-20000));

            //Act
            var aggregatedPosition = _aggregatorServiceHelper.CreateAggregatedPosition(@event);

            //Assert
            aggregatedPosition.Latitude.Should().Be(@event.Latitude);
            aggregatedPosition.Longitude.Should().Be(@event.Longitude);
            aggregatedPosition.Height.Should().Be(@event.Height);
            aggregatedPosition.NewestTime.Should().Be(@event.CreateDateTime);
            aggregatedPosition.OldestTime.Should().BeNull();
        }

        [TestMethod]
        public void
            AverageWithAggregatedPosition_GivenPositionCreatedEvent_IfCanBeAggregated_ReturnTrue()
        {
            //Arrange
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-20300));
            var aggregatedPosition = new AggregatedPosition(52.57, 5.93, 4.09,
                DateTime.UtcNow.AddMilliseconds(-20100),
                DateTime.UtcNow.AddMilliseconds(-19500));

            //Act
            var aggregated =
                _aggregatorServiceHelper.AverageWithAggregatedPosition(aggregatedPosition, @event);

            //Assert
            aggregated.Should().BeTrue();
        }

        [TestMethod]
        public void
            AverageWithAggregatedPosition_GivenPositionCreatedEvent_IfCanNotBeAggregated_ReturnFalse()
        {
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-15000));
            var aggregatedPosition = new AggregatedPosition(52.57, 5.93, 4.09,
                DateTime.UtcNow.AddMilliseconds(-20100),
                DateTime.UtcNow.AddMilliseconds(-19500));

            //Act
            var aggregated =
                _aggregatorServiceHelper.AverageWithAggregatedPosition(aggregatedPosition, @event);

            //Assert
            aggregated.Should().BeFalse();
        }


        [TestMethod]
        public void GetAggregatedPositionForCreationTime_GivenEventThatCanBeAggregated_ReturnsAggregatePosition()
        {
            //Arrange
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-20300));
            var recentAggregatedPositions = SetupAggregatedPositions();

            _aggregatedPositionRepositoryMock.Setup(x => x.GetLastFive()).Returns(recentAggregatedPositions);

            //Act
            var actualAggregatedPosition = _aggregatorServiceHelper.GetAggregatedPositionForCreationTime(
                recentAggregatedPositions,
                @event.CreateDateTime);

            //Assert
            actualAggregatedPosition.Should().NotBeNull();
        }

        [TestMethod]
        public void GetAggregatedPositionForCreationTime_GivenEventThatCanNotBeAggregated_ReturnsNull()
        {
            //Arrange
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-8300));
            var recentAggregatedPositions = SetupAggregatedPositions();

            _aggregatedPositionRepositoryMock.Setup(x => x.GetLastFive()).Returns(recentAggregatedPositions);

            //Act
            var actualAggregatedPosition = _aggregatorServiceHelper.GetAggregatedPositionForCreationTime(
                recentAggregatedPositions,
                @event.CreateDateTime);

            //Assert
            actualAggregatedPosition.Should().BeNull();
        }

        [TestMethod]
        public void IsDelayedEvent_GivenEventThatIsEarlierThanProvidedAggregatedPositions_ReturnsTrue()
        {
            //Arrange
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-28300));
            var recentAggregatedPositions = SetupAggregatedPositions();

            _aggregatedPositionRepositoryMock.Setup(x => x.GetLastFive()).Returns(recentAggregatedPositions);

            //Act
            var isDelayed = _aggregatorServiceHelper.IsDelayedEvent(
                recentAggregatedPositions,
                @event.CreateDateTime);

            //Assert
            isDelayed.Should().BeTrue();
        }

        [TestMethod]
        public void IsDelayedEvent_GivenEventThatIsNotEarlierThanProvidedAggregatedPositions_ReturnsFalse()
        {
            //Arrange
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-17300));
            var recentAggregatedPositions = SetupAggregatedPositions();

            _aggregatedPositionRepositoryMock.Setup(x => x.GetLastFive()).Returns(recentAggregatedPositions);

            //Act
            var isDelayed = _aggregatorServiceHelper.IsDelayedEvent(
                recentAggregatedPositions,
                @event.CreateDateTime);

            //Assert
            isDelayed.Should().BeFalse();
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
