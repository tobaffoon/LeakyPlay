namespace LeakyPlayTgBotNet.ReceiveService;

/// <summary>
/// A marker interface for Update Receiver service
/// </summary>
public interface IReceiverService
{
   Task ReceiveAsync(CancellationToken cancellationToken);
}
