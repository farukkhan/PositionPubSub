namespace Application
{
    public class Settings
    {
        public int BroadcastFrequencyMilliSecs => GetRandomBroadcastFrequencyMilliSecs();

        public int MinBroadcastRangeMilliSec{ get; set; }

        public int MaxBroadcastRangeMilliSec{ get; set; }

        public int DelayInMilliSec{ get; set; }

        private int GetRandomBroadcastFrequencyMilliSecs()
        {
            var random = new Random();
            return random.Next(MinBroadcastRangeMilliSec, MaxBroadcastRangeMilliSec);
        }
    }
}
