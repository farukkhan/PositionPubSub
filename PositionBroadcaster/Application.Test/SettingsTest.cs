namespace Application.Test
{
    [TestClass]
    public class SettingsTest
    {
        [TestMethod]
        public void BroadcastFrequency_GeneratesNumber_InTheRange()
        {
            var settings = new Settings();

            Assert.IsTrue(settings.BroadcastFrequencyMilliSecs >= 1010 || settings.BroadcastFrequencyMilliSecs <= 5000);
        }
    }
}