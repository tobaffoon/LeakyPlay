using LeakyPlayTgBotNet.ReceiveService;

namespace LeakyPlayTgBotNet.PollService;

public class TelegramPollingService(IServiceProvider serviceProvider, ILogger<TelegramPollingService> logger)
: PollingService<LeakyTelegramReceiverService>(serviceProvider, logger);
