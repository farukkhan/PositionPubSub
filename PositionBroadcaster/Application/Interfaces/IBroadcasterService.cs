namespace Application.Interfaces;

public interface IBroadcasterService : IDisposable
{
    void StartBroadcasting(bool simulateDelay);
}