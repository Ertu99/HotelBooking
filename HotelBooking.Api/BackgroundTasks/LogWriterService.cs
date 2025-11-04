
namespace HotelBooking.Api.BackgroundTasks
{
    public class LogWriterService : BackgroundService
    {
        private readonly ILogger<LogWriterService> _logger;
        public LogWriterService(ILogger<LogWriterService> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LogWriterService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background job running at: {time}", DateTime.Now);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation("LogWriterService stopped.");
        }
    }
}
