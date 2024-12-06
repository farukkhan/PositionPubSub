namespace Application.Test
{
    [TestClass]
    public class SettingsTest
    {
        [TestMethod]
        public void BroadcastFrequency_GeneratesNumber_InTheRange()
        {
            var settings = new Settings();

            Assert.IsTrue(settings.BroadcastFrequency >= 1.01 || settings.BroadcastFrequency <= 5);
        }
    }
}