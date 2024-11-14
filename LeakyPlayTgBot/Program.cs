using Microsoft.Extensions.Configuration;
using LeakyPlayTgBot.TelegramBot;

namespace LeakyPlayTelegramBot
{
   class Program
   {
      private const string ConfigTokenKey = "TgBotConfig:BotToken";
      static async Task Main(string[] args)
      {
         // wrapper for user secrets
         IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

         if (config[ConfigTokenKey] is not string botToken)
         {
            throw new ArgumentNullException($"{ConfigTokenKey} is not defined correctly");
         }

         var cts = new CancellationTokenSource();
         var leakyBot = new LeakyPlayBot(botToken, cts.Token);
         leakyBot.StartBot();
         Console.ReadLine();
      }
   }
}