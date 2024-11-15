using LeakyPlayTgBotNet.PollService;
using LeakyPlayTgBotNet.ReceiveService;
using LeakyPlayTgBotNet.UpdateService;
using LeakyPlayTgBotNet.TelegramBot;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace LeakyPlayTgBotNet
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

         Console.WriteLine(config.GetRequiredSection(ConfigTokenKey));
         IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
               // TODO: Register Bot configuration
               // services.Configure<TgBotConfig>(config.GetRequiredSection(ConfigTokenKey));

               services.AddHttpClient("leakyplay_tg_bot_client").RemoveAllLoggers()
                  .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                  {
                     // TODO: redo config creation using service provider
                     string? botToken = config.GetValue<string>(ConfigTokenKey);
                     ArgumentNullException.ThrowIfNull(botToken);

                     TgBotConfig botConfig = new TgBotConfig
                     {
                        BotToken = botToken
                     };
                     ArgumentNullException.ThrowIfNull(botConfig);
                     TelegramBotClientOptions options = new TelegramBotClientOptions(botConfig.BotToken);
                     return new TelegramBotClient(options, httpClient);
                  });

               services.AddScoped<UpdateHandler>();
               services.AddScoped<ReceiverService>();
               services.AddHostedService<PollingService>();
            })
            .Build();

         await host.RunAsync();
      }
   }
}