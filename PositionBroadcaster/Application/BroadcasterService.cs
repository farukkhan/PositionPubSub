namespace Application
{
    public class BroadcasterService : IBroadcasterService
    {
        private readonly ISettings _settings;

        public BroadcasterService(ISettings settings)
        {
            _settings = settings;
        }

        public void StartBroadcasting()
        {
            _ = new Timer(Broadcast, null, 0, _settings.BroadcastFrequencyMilliSecs);
        }

        private void Broadcast(object state)
        {
        }
    }
}
