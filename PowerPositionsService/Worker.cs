using Microsoft.Extensions.Options;
using PowerPositionsService.Models;

namespace PowerPositionsService
{
    public class Worker : BackgroundService
    {
        private readonly IPowerServiceProcessor _powerServiceProcessor;
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<PowerPositionsSettings> _options;

        public Worker(IPowerServiceProcessor powerServiceProcessor, ILogger<Worker> logger, IOptions<PowerPositionsSettings> options)
        {
            _powerServiceProcessor = powerServiceProcessor;
            _logger = logger;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting PowerPositions service with {_options.Value.IntervalInSeconds} seconds interval.");

            var timer = new PeriodicTimer(TimeSpan.FromSeconds(_options.Value.IntervalInSeconds));

            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessPositions();

                await timer.WaitForNextTickAsync(stoppingToken);
            }
        }

        private void ProcessPositions()
        {
            Task.Run(async () =>
            {
                _logger.LogInformation("Processing positions...");

                for (int i = 0; i < _options.Value.MaxRetryCount; i++)
                {
                    try
                    {
                        await _powerServiceProcessor.Process(DateTime.Today);
                        _logger.LogInformation("Finished processing positions...");
                        return;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"Process failed on attempt {i}: {ex}");
                    }
                }

                // TODO: Notify failure
                _logger.LogError($"Failed to process positions after {_options.Value.MaxRetryCount} attempts");
            });
        }
    }
}