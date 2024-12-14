namespace LeakyPlayTgBotNet.UpdateService
{
   using Telegram.Bot;
   using Telegram.Bot.Polling;
   using Telegram.Bot.Types;
   using Telegram.Bot.Types.Enums;

   public class UpdateHandler(ILogger<UpdateHandler> logger) : IUpdateHandler
   {
      private readonly ILogger<UpdateHandler> _logger = logger;

      public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         _logger.LogInformation("HandleError: {Exception}", exception);
      }

      public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
      {
         cancellationToken.ThrowIfCancellationRequested();
         await (update switch
         {
            { Message: { } message } => OnMessage(botClient, message),
            { CallbackQuery: { } callbackQuery } => OnCallbackQuery(botClient, callbackQuery),
            _ => UnknownUpdateHandlerAsync(update)
         });
      }

      private async Task OnMessage(ITelegramBotClient botClient, Message msg)
      {
         _logger.LogInformation("Receive message type: {MessageType}", msg.Type);
         if (msg.Text == null)
         {
            return;
         }

         string[] words = msg.Text.Split(' ');
         Message sentMessage = await (words[0] switch
         {
            "/help" => SendHelp(botClient, msg),
            "/create" => CreateRoom(botClient, msg),
            "/enter" => EnterRoom(botClient, msg),
            "/change_room" => ChangeRoom(botClient, msg),
            "/leave" => LeaveRoom(botClient, msg),
            "/add" => AddPlaylist(botClient, msg),
            "/delete" => SendDeleteInline(botClient, msg),
            "/combine" => Combine(botClient, msg),
            "/artists" => SendArtists(botClient, msg),
            "/playlists" => SendPlaylists(botClient, msg),
            "/members" => SendMembers(botClient, msg),
            _ => OnUnknownCommand(botClient, msg)
         });

         _logger.LogInformation("The Response was sent with id: {SentMessageId}", sentMessage.Id);
      }

      private Task<Message> OnUnknownCommand(ITelegramBotClient botClient, Message msg)
      {
         const string getHelp = """
         Unknown command =/
         Enter /help to get list of commands
         """;
         return botClient.SendMessage(msg.Chat, getHelp, replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> SendHelp(ITelegramBotClient botClient, Message msg)
      {
         const string usage = """
               <b><u>Bot menu</u></b>:
            
            /help - Get list of commands
            /create - Create virtual room with single combined playlist
            /enter - Pass &lt;roomID&gt; to enter existing virtual room 
            /change_room - Pass &lt;roomID&gt; to enter a new room
            /leave - Leave current room
            /add - Pass &lt;link&gt; to add a playlist to combination list by link
            /delete - See combination list and choose a playlist to delete
            /combine - Update combined playlist
            /artists - Show all unique artists in current combined playlist
            /playlists - Show all playlists in combination list
            /members - Show all users in same room with you
            """;
         return botClient.SendMessage(msg.Chat, usage, parseMode: ParseMode.Html, replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> CreateRoom(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! Room created", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> EnterRoom(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! Room entered", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> ChangeRoom(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! Room changed", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> LeaveRoom(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! You left room", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> AddPlaylist(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! You added playlist", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> SendDeleteInline(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! You started delete with inline keyboard", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> TryDeletePlaylist(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! You tried deleting a playlist", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> Combine(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! You combined playlists", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> SendArtists(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! You recieved artists list", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> SendPlaylists(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! You recieved playlists list", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      private Task<Message> SendMembers(ITelegramBotClient botClient, Message msg)
      {
         return botClient.SendMessage(msg.Chat, "WOW! You recieved members list", replyParameters: new ReplyParameters { MessageId = msg.Id });
      }

      // Process Inline Keyboard callback data
      // TODO implement
      private Task OnCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
      {
         _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);
         return botClient.SendMessage(callbackQuery.Message!.Chat, $"Received {callbackQuery.Data}");
      }

      private Task UnknownUpdateHandlerAsync(Update update)
      {
         _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
         return Task.CompletedTask;
      }
   }
}