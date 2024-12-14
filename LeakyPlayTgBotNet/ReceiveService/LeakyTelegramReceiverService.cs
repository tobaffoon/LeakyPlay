using LeakyPlayTgBotNet.UpdateService;
using Telegram.Bot;

namespace LeakyPlayTgBotNet.ReceiveService;

public class LeakyTelegramReceiverService(ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<LeakyTelegramReceiverService> logger)
   : TelegramReceiverService<UpdateHandler>(botClient, updateHandler, logger);