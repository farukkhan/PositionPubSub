namespace Domain.Test
{
    [TestClass]
    public class PositionTest
    {
        [TestMethod]
        public void CreatePosition_CreatesPosition_InsideOfNetherlands()
        {
            var position = Position.CreatePosition();

            Assert.IsTrue(position.Latitude >= 50.77083d && position.Latitude <= 53.35917);
            Assert.IsTrue(position.Longitude >= 3.57361 && position.Longitude <= 7.10833);
        }
    }
}