using System;
using LeakyPlayTgBot.UserSession;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LeakyPlayTgBot.TelegramBot;

public class LeakyPlayBot
{
   private readonly Dictionary<int, Session> _userSessions = new Dictionary<int, Session>();
   private readonly CancellationToken _cancellationToken;
   private readonly TelegramBotClient _client;
   public bool IsStarted
   {
      get;
      private set;
   }
   public LeakyPlayBot(string telegramBotToken, CancellationToken cancellationToken)
   {
      _client = new TelegramBotClient(telegramBotToken);
      _cancellationToken = cancellationToken;
   }

   public void StartBot()
   {
      if (IsStarted)
      {
         throw new InvalidOperationException("Attempt to start server that is already running");
      }
      ReceiverOptions receiverOptions = new()
      {
         AllowedUpdates = []
      };

      _client.StartReceiving(Update, Error, receiverOptions: receiverOptions, cancellationToken: _cancellationToken);
   }

   private async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
   {
      var message = update.Message;
      if (message is null)
      {
         return;
      }

      string response = message.Text switch
      {
         "/start" => "a",
         _ => @"Unknown command =/
                  Enter /help to get list of commands"
      };


      await client.SendTextMessageAsync(message.Chat.Id, response, replyMarkup: GetButtons());
      return;
   }

   private async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
   {
      throw new NotImplementedException();
   }

   private IReplyMarkup GetButtons()
   {
      return new InlineKeyboardMarkup([
          InlineKeyboardButton.WithCallbackData("Info", "Help"),
                InlineKeyboardButton.WithCallbackData("Enter a room", "hint1")
      ]);
   }
}
