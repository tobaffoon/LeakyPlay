using LeakyPlayTgBotNet.UpdateService;
using Telegram.Bot;

namespace LeakyPlayTgBotNet.ReceiveService;

// Compose Receiver and UpdateHandler implementation
public class ReceiverService(ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);
