using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LeakyPlayTgBotNet.UpdateService;

public class UpdateHandler : IUpdateHandler
{
   private readonly ITelegramBotClient _bot;
   private readonly ILogger<UpdateHandler> _logger;
   public UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger)
   {
      _bot = bot;
      _logger = logger;
   }

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
         { Message: { } message } => OnMessage(message),
         { CallbackQuery: { } callbackQuery } => OnCallbackQuery(callbackQuery),
         _ => UnknownUpdateHandlerAsync(update)
      });
   }

   private async Task OnMessage(Message msg)
   {
      _logger.LogInformation("Receive message type: {MessageType}", msg.Type);
      if (msg.Text is not string messageText)
         return;

      string[] words = messageText.Split(' ');
      Message sentMessage = await (words[0] switch
      {
         "/help" => SendHelp(msg),
         "/create" => CreateRoom(msg),
         "/enter" => EnterRoom(msg),
         "/change_room" => ChangeRoom(msg),
         "/leave" => LeaveRoom(msg),
         "/add" => AddPlaylist(msg),
         "/delete" => SendDeleteInline(msg),
         "/delete_id" => TryDeletePlaylist(msg),
         "/combine" => Combine(msg),
         "/artists" => SendArtists(msg),
         "/playlists" => SendPlaylists(msg),
         "/members" => SendMembers(msg),
         _ => OnUnknownCommand(msg)
      });
      _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.Id);
   }

   async Task<Message> OnUnknownCommand(Message msg)
   {
      const string getHelp = """
               Unknown command =/
               Enter /help to get list of commands
      """;
      return await _bot.SendMessage(msg.Chat, getHelp);
   }

   async Task<Message> SendHelp(Message msg)
   {
      const string usage = """
               <b><u>Bot menu</u></b>:
               /help - Get list of commands
               /create - Create virtual room with single combined playlist
               /enter - Pass <roomID> to enter existing virtual room 
               /change_room - Pass <roomID> to enter a new room
               /leave - Leave current room
               /add - Pass <link> to add a playlist to combination list by link
               /delete - See combination list and choose a playlist to delete
               /delete_id - Pass <playlistId> to delete a playlist
               /combine - Update combined playlist
               /artists - Show all unique artists in current combined playlist
               /playlists - Show all playlists in combination list
               /members - Show all users in same room with you
            """;
      return await _bot.SendMessage(msg.Chat, usage, parseMode: ParseMode.Html);
   }

   async Task<Message> CreateRoom(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! Room created");
   }

   async Task<Message> EnterRoom(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! Room entered");
   }

   async Task<Message> ChangeRoom(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! Room changed");
   }

   async Task<Message> LeaveRoom(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! You left room");
   }

   async Task<Message> AddPlaylist(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! You added playlist");
   }

   async Task<Message> SendDeleteInline(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! You started delete with inline keyboard");
   }

   async Task<Message> TryDeletePlaylist(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! You tried deleting a playlist");
   }

   async Task<Message> Combine(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! You combined playlists");
   }

   async Task<Message> SendArtists(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! You recieved artists list");
   }

   async Task<Message> SendPlaylists(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! You recieved playlists list");
   }

   async Task<Message> SendMembers(Message msg)
   {
      return await _bot.SendMessage(msg.Chat, "WOW! You recieved members list");
   }

   // Process Inline Keyboard callback data
   private async Task OnCallbackQuery(CallbackQuery callbackQuery)
   {
      _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);
      await _bot.SendMessage(callbackQuery.Message!.Chat, $"Received {callbackQuery.Data}");
   }

   private Task UnknownUpdateHandlerAsync(Update update)
   {
      _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
      return Task.CompletedTask;
   }
}