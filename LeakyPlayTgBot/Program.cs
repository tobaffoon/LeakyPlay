using Microsoft.Extensions.Configuration;
using LeakyPlayTgBot.TelegramBot;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;


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

         IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
               // Register Bot configuration
               services.Configure<BotConfiguration>(config.GetRequiredSection(ConfigTokenKey));

               services.AddHttpClient("leakyplay_tg_bot_client").RemoveAllLoggers()
                  .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                  {
                     BotConfiguration? botConfiguration = sp.GetService<IOptions<BotConfiguration>>()?.Value;
                     ArgumentNullException.ThrowIfNull(botConfiguration);
                     TelegramBotClientOptions options = new TelegramBotClientOptions(botConfiguration.BotToken);
                     return new TelegramBotClient(options, httpClient);
                  });

               services.AddScoped<UpdateHandler>();
               services.AddScoped<ReceiverService>();
               services.AddHostedService<PollingService>();
            })
            .Build();
      }
   }
}