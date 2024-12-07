namespace Application
{
    public class Settings : ISettings
    {
        public int BroadcastFrequencyMilliSecs{ get; }

        public Settings()
        {
            BroadcastFrequencyMilliSecs = GetRandomBroadcastFrequencyMilliSecs();
        }


        private int GetRandomBroadcastFrequencyMilliSecs()
        {
            var random = new Random();
            return random.Next(1010, 5000);
        }
    }
}
