namespace LeakyPlayTgBotNet.ReceiveService
{
   using LeakyPlayTgBotNet.UpdateService;
   using Telegram.Bot;

   public class LeakyTelegramReceiverService(ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<LeakyTelegramReceiverService> logger)
      : TelegramReceiverService<UpdateHandler>(botClient, updateHandler, logger);
}