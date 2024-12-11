using Application.Interfaces;
using Application.Processes;
using Moq;

namespace Application.Test
{
    [TestClass]
    public class PositionConsumerProcessTest
    {
        private readonly Mock<IEventBus> _eventBusMock = new();
        private readonly Mock<IPositionAggregatorProcess> _positionAggregatorProcessMock = new();

        [TestMethod]
        public void StartAsync_CallsTheBusToReceive()
        {
            var positionReceiverService =
                new PositionConsumerProcess(_eventBusMock.Object, _positionAggregatorProcessMock.Object);

            positionReceiverService.StartAsync();

            _eventBusMock.Verify(x => x.StartAsync(It.IsAny<CancellationToken>()), Times.Once);
            _positionAggregatorProcessMock.Verify(x => x.StartAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}