using LeakyPlayTgBotNet.ReceiveService;

namespace LeakyPlayTgBotNet.PollService;

/// <summary>
/// An abstract class to compose Polling background service and Receiver implementation classes
/// </summary>
/// <typeparam name="TReceiverService">Receiver implementation class</typeparam>
public abstract class PollingServiceBase<TReceiverService> : BackgroundService
    where TReceiverService : IReceiverService
{
   private readonly IServiceProvider _serviceProvider;
   private readonly ILogger _logger;

   internal PollingServiceBase(
       IServiceProvider serviceProvider,
       ILogger<PollingServiceBase<TReceiverService>> logger)
   {
      _serviceProvider = serviceProvider;
      _logger = logger;
   }

   protected override async Task ExecuteAsync(CancellationToken cancellationToken)
   {
      _logger.LogInformation("Starting polling service");

      await DoWork(cancellationToken);
   }

   private async Task DoWork(CancellationToken cancellationToken)
   {
      // Make sure we receive updates until Cancellation Requested,
      // no matter what errors our ReceiveAsync get
      while (!cancellationToken.IsCancellationRequested)
      {
         try
         {
            // Create new IServiceScope on each iteration.
            // This way we can leverage benefits of Scoped TReceiverService
            // and typed HttpClient - we'll grab "fresh" instance each time
            using var scope = _serviceProvider.CreateScope();
            var receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();

            await receiver.ReceiveAsync(cancellationToken);
         }
         catch (Exception ex)
         {
            _logger.LogError("Polling failed with exception: {Exception}", ex);

            // Cooldown if something goes wrong
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
         }
      }
   }
}