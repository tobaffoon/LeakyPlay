using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace LeakyPlayTelegramBot
{
   class Program
   {
      public const string ConfigTokenKey = "TgBotConfig:BotToken";
      static async Task Main(string[] args)
      {
         // wrapper for user secrets
         IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

         if (config[ConfigTokenKey] is not string token)
         {
            throw new ArgumentNullException($"{ConfigTokenKey} is not defined correctly");
         }

         var client = new TelegramBotClient(token);
         ReceiverOptions receiverOptions = new()
         {
            AllowedUpdates = []
         };
         client.StartReceiving(Update, Error, receiverOptions: receiverOptions);
         Console.ReadLine();
      }

      private static async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
      {
         System.Console.WriteLine("New req");
         if (update.Message == null)
         {
            //aa
            return;
         }

         var message = update.Message;
         if (message.Text == null)
         {
            //b
            System.Console.WriteLine("No Text");
         }
         else if (message.Text.ToLower() == "help")
         {
            await client.SendTextMessageAsync(message.Chat.Id, "Nuh uh");
         }
         else
         {
            await client.SendTextMessageAsync(message.Chat.Id, "I don't understant", replyMarkup: GetButtons());
         }

         if (message.Photo == null)
         {
            // c
            System.Console.WriteLine("No Photo");
         }
         else
         {

            await client.SendTextMessageAsync(message.Chat.Id, $"I Don't like this photo, dear {message.Chat.FirstName}");
         }


         return;
      }

      private static async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
      {
         throw new NotImplementedException();
      }

      private static IReplyMarkup GetButtons()
      {
         return new InlineKeyboardMarkup([
             InlineKeyboardButton.WithCallbackData("Info", "Help"),
                InlineKeyboardButton.WithCallbackData("Enter a room", "hint1")
         ]);
      }
   }
}