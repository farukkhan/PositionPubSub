namespace Application
{
    public class Settings : ISettings
    {
        public double BroadcastFrequency{ get; }

        public Settings()
        {
            BroadcastFrequency = GetRandomBroadcastFrequency();
        }

        private double GetRandomBroadcastFrequency()
        {
            var random = new Random();
            var randomvalue = random.NextDouble() * (5 - 1.01) + 1.01;

            return randomvalue;
        }
    }
}
