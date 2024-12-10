using Application.Interfaces;
using IntergrationEvents;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Test
{
    [TestClass]
    public class BroadcasterServiceTest
    {
        private readonly Mock<IOptionsMonitor<Settings>> _settingsOptionMock = new();
        private readonly Mock<IEventBus> _eventBusMock = new();

        [TestMethod]
        public async Task StartBroadcasting_CreatesPositionAndPublishes()
        {
            var settings = new Settings
            {
                MaxBroadcastRange = 200,
                MinBroadcastRange = 150
            };

            _settingsOptionMock.Setup(x => x.CurrentValue).Returns(settings);

            using (var broadcasterService =
                   new BroadcasterService(_settingsOptionMock.Object, _eventBusMock.Object))
            {
                broadcasterService.StartBroadcasting();

                await Task.Delay(500);
            }

            _eventBusMock.Verify(e => e.PublishAsync(It.IsAny<PositionCreatedIntegrationEvent>()), Times.AtLeast(2));
        }
    }
}