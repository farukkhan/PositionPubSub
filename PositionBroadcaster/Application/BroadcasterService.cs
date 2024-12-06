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
            Console.WriteLine(@"Started Broadcasting");
        }
    }
}
