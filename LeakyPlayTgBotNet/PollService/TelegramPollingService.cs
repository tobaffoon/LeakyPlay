namespace LeakyPlayTgBotNet.PollService
{
   using LeakyPlayTgBotNet.ReceiveService;

   public class TelegramPollingService(IServiceProvider serviceProvider, ILogger<TelegramPollingService> logger)
   : PollingService<LeakyTelegramReceiverService>(serviceProvider, logger);
}