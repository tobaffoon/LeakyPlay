namespace LeakyPlayTgBotNet
{
   using LeakyPlayTgBotNet.ReceiveService;
   using LeakyPlayTgBotNet.UpdateService;
   using LeakyPlayTgBotNet.TelegramBot;
   using Telegram.Bot;
   using LeakyPlayTgBotNet.PollService;

   class Program
   {
      private const string ConfigTokenKey = "TgBotConfig:BotToken";
      private const string ConfigServerPort = "TgBotConfig:ServerPort";

      static async Task Main(string[] args)
      {
         // wrapper for user secrets
         IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

         IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
               services.AddHttpClient("leakyplay_tg_bot_client")
                  .RemoveAllLoggers()
                  .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                  {
                     string? botToken = config.GetValue<string>(ConfigTokenKey);
                     ArgumentNullException.ThrowIfNull(botToken);
                     TgBotConfig botConfig = new TgBotConfig
                     {
                        BotToken = botToken
                     };

                     TelegramBotClientOptions options = new TelegramBotClientOptions(botConfig.BotToken);
                     return new TelegramBotClient(options, httpClient);
                  });

               services.AddScoped<UpdateHandler>();
               services.AddScoped<LeakyTelegramReceiverService>();
               services.AddHostedService<TelegramPollingService>();
            })
            .Build();

         await host.RunAsync();
      }
   }
}