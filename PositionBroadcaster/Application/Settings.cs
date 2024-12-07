using Application.Interfaces;

namespace Application
{
    public class Settings : ISettings
    {
        public int BroadcastFrequencyMilliSecs => GetRandomBroadcastFrequencyMilliSecs();

        private static int GetRandomBroadcastFrequencyMilliSecs()
        {
            var random = new Random();
            return random.Next(1010, 5000);
        }
    }
}
