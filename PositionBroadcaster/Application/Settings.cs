namespace Application
{
    public class Settings
    {
        public int BroadcastFrequencyMilliSecs => GetRandomBroadcastFrequencyMilliSecs();

        public int MinBroadcastRange{ get; set; }

        public int MaxBroadcastRange{ get; set; }

        private int GetRandomBroadcastFrequencyMilliSecs()
        {
            var random = new Random();
            return random.Next(MinBroadcastRange, MaxBroadcastRange);
        }
    }
}
