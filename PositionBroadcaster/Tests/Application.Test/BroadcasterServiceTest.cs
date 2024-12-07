using Application.Interfaces;
using Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Test
{
    [TestClass]
    public class BroadcasterServiceTest
    {
        private readonly Mock<ISettings> _settingsMock = new();
        private readonly Mock<IEventBus> _eventBusMock = new();

        [TestMethod]
        public async Task StartBroadcasting_CreatesPositionAndPublishes()
        {
            _settingsMock.Setup(s => s.BroadcastFrequencyMilliSecs).Returns(200);

            using (var broadcasterService =
                   new BroadcasterService(_settingsMock.Object, _eventBusMock.Object))
            {
                broadcasterService.StartBroadcasting();

                await Task.Delay(500);
            }

            _eventBusMock.Verify(e => e.PublishAsync(It.IsAny<PositionCreatedEvent>()), Times.AtLeast(2));
        }
    }
}