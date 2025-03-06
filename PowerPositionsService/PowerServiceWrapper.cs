using Services;

namespace PowerPositionsService
{
    public interface IPowerServiceWrapper
    {
        Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime tradeDay);
    }

    public class PowerServiceWrapper : IPowerServiceWrapper
    {
        private readonly IPowerService _powerService;
        private readonly ILogger<PowerServiceWrapper> _logger;

        public PowerServiceWrapper(IPowerService powerService, ILogger<PowerServiceWrapper> logger)
        {
            _powerService = powerService;
            _logger = logger;
        }

        public async Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime tradeDay)
        {
            var powerTrades = (await _powerService.GetTradesAsync(tradeDay.Date)).ToList();

            _logger.LogInformation($"Received {powerTrades.Count} trades from PowerService");

            return powerTrades;
        }
    }
}