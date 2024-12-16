namespace LeakyPlayTgBotNet.ReceiveService
{
   using Telegram.Bot;
   using Telegram.Bot.Polling;

   /// <summary>
   /// A class to compose Receiver Service and Telegram client
   /// </summary>
   /// <typeparam name="TUpdateHandler">Update Handler to use in Update Receiver</typeparam>
   public class TelegramReceiverService<TUpdateHandler> : IReceiverService
       where TUpdateHandler : IUpdateHandler
   {
      private readonly ITelegramBotClient _botClient;
      private readonly IUpdateHandler _updateHandler;
      private readonly ILogger<TelegramReceiverService<TUpdateHandler>> _logger;

      internal TelegramReceiverService(
          ITelegramBotClient botClient,
          TUpdateHandler updateHandler,
          ILogger<TelegramReceiverService<TUpdateHandler>> logger)
      {
         _botClient = botClient;
         _updateHandler = updateHandler;
         _logger = logger;
      }

      /// <summary>
      /// Start to service Updates with provided Update Handler class
      /// </summary>
      /// <param name="cancellationToken"></param>
      /// <returns></returns>
      public async Task ReceiveAsync(CancellationToken cancellationToken)
      {
         // ToDo: we can inject ReceiverOptions through IOptions container
         var receiverOptions = new ReceiverOptions()
         {
            AllowedUpdates = [],
            DropPendingUpdates = true,
         };

         var me = await _botClient.GetMe(cancellationToken);
         _logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "Unnamed bot");

         // Start receiving updates
         await _botClient.ReceiveAsync(
             updateHandler: _updateHandler,
             receiverOptions: receiverOptions,
             cancellationToken: cancellationToken);
      }
   }
}