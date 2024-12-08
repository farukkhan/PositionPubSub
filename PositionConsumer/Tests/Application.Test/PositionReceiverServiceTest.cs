using Application.Interfaces;
using Moq;

namespace Application.Test
{
    [TestClass]
    public class PositionReceiverServiceTest
    {
        private readonly Mock<IEventBus> _eventBusMock = new();

        [TestMethod]
        public void StartAsync_CallsTheBusToReceive()
        {
            var positionReceiverService = new PositionReceiverService(_eventBusMock.Object);

            positionReceiverService.StartAsync();

            _eventBusMock.Verify(x => x.ReceiveAsync(), Times.Once);
        }
    }
}