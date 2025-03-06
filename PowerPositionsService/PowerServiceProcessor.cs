using PowerPositionsService.Reporting;
using Services;

namespace PowerPositionsService
{
    public interface IPowerServiceProcessor
    {
        Task Process(DateTime tradeDay);
    }

    public class PowerServiceProcessor : IPowerServiceProcessor
    {
        private readonly IPowerServiceWrapper _powerServiceWrapper;
        private readonly ITradesAggregator _tradesAggregator;
        private readonly IPowerPeriodToPowerTimeConverter _converter;
        private readonly IReportGenerator _reportGenerator;

        public PowerServiceProcessor(IPowerServiceWrapper powerServiceWrapper, ITradesAggregator tradesAggregator, IPowerPeriodToPowerTimeConverter converter, IReportGenerator reportGenerator)
        {
            _powerServiceWrapper = powerServiceWrapper;
            _tradesAggregator = tradesAggregator;
            _converter = converter;
            _reportGenerator = reportGenerator;
        }

        public async Task Process(DateTime tradeDay)
        {
            var trades = await _powerServiceWrapper.GetTradesAsync(tradeDay);

            var totalPowerPeriods = _tradesAggregator.Aggregate(trades);

            var powerTimes = totalPowerPeriods.Select(t => _converter.Convert(t, tradeDay)).ToList();

            _reportGenerator.GenerateRport(powerTimes);
        }
    }
}