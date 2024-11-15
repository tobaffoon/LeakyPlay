using LeakyPlayTgBotNet.ReceiveService;

namespace LeakyPlayTgBotNet.PollService;

// Compose Polling and ReceiverService implementations
public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);
