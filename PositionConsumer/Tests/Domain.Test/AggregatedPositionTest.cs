using Domain.Entities;
using FluentAssertions;
using IntergrationEvents;

namespace Domain.Test
{
    [TestClass]
    public class AggregatedPositionTest
    {
        [TestMethod]
        public void
            DoAverage_GivenPositionCanBeAggregated_DoAverageAndReturnsTrue()
        {
            //Arrange
            var aggregatedPosition = new AggregatedPosition(51.57, 6.83, 4.09, DateTime.UtcNow.AddMilliseconds(-20000),
                DateTime.UtcNow.AddMilliseconds(-19500));
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-19700));

            //Act
            var result = aggregatedPosition.DoAverage(@event.Latitude, @event.Longitude, @event.Height,
                @event.CreateDateTime);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void
            DoAverage_GivenPositionCanBeNotAggregated_DoAverageAndReturnsFalse()
        {
            //Arrange
            var aggregatedPosition = new AggregatedPosition(51.57, 6.83, 4.09, DateTime.UtcNow.AddMilliseconds(-20000),
                DateTime.UtcNow.AddMilliseconds(-19500));
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-21100));

            //Act
            var result = aggregatedPosition.DoAverage(@event.Latitude, @event.Longitude, @event.Height,
                @event.CreateDateTime);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void
            CanBeAggregated_GivenPositionOldestTimeIsNull_And_CreateDateTimeIsWithinAcceptableTimeSpan_ReturnsTrue()
        {
            //Arrange
            var aggregatedPosition = new AggregatedPosition(51.57, 6.83, 4.09, DateTime.UtcNow.AddMilliseconds(-20000),
                null);
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-19700));

            //Act
            var result = aggregatedPosition.CanBeAggregated(@event.CreateDateTime);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void
            CanBeAggregated_GivenPositionHaveNewestAndOldestTime_And_CreateDateTimeIsWithinNewestAndOldest_ReturnsTrue()
        {
            //Arrange
            var aggregatedPosition = new AggregatedPosition(51.57, 6.83, 4.09, DateTime.UtcNow.AddMilliseconds(-20000),
                DateTime.UtcNow.AddMilliseconds(-19500));
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-19700));

            //Act
            var result = aggregatedPosition.CanBeAggregated(@event.CreateDateTime);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void
            CanBeAggregated_GivenCreateTimeIsLaterThanNewestAndOldestTime_But_CreateDateTimeIsWithinAcceptableTimeSpan_ReturnsTrue()
        {
            //Arrange
            var aggregatedPosition = new AggregatedPosition(51.57, 6.83, 4.09, DateTime.UtcNow.AddMilliseconds(-20000),
                DateTime.UtcNow.AddMilliseconds(-19500));
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-19900));

            //Act
            var result = aggregatedPosition.CanBeAggregated(@event.CreateDateTime);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void
            CanBeAggregated_GivenCreateTimeIsEarlierThanNewestAndOldestTime_But_CreateDateTimeIsWithinAcceptableTimeSpan_ReturnsTrue()
        {
            //Arrange
            var aggregatedPosition = new AggregatedPosition(51.57, 6.83, 4.09, DateTime.UtcNow.AddMilliseconds(-20000),
                DateTime.UtcNow.AddMilliseconds(-19500));
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-20400));

            //Act
            var result = aggregatedPosition.CanBeAggregated(@event.CreateDateTime);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void CanBeAggregated_GivenCreateTimeIsNotWithinAcceptableTimeSpan_ReturnsFalse()
        {
            //Arrange
            var aggregatedPosition = new AggregatedPosition(51.57, 6.83, 4.09, DateTime.UtcNow.AddMilliseconds(-20000),
                DateTime.UtcNow.AddMilliseconds(-19500));
            var @event = new PositionCreatedIntegrationEvent(Guid.NewGuid(), 51.57, 6.83, 4.09,
                DateTime.UtcNow.AddMilliseconds(-20600));

            //Act
            var result = aggregatedPosition.CanBeAggregated(@event.CreateDateTime);

            //Assert
            result.Should().BeFalse();
        }
    }
}