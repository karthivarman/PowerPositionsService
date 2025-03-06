using Services;

namespace PowerPositionsService
{
    public interface ITradesAggregator
    {
        IEnumerable<PowerPeriod> Aggregate(IEnumerable<PowerTrade> trades);
    }

    public class HourlyTradesAggregator : ITradesAggregator
    {
        public IEnumerable<PowerPeriod> Aggregate(IEnumerable<PowerTrade> trades)
        {
            for (var i = 1; i <= 24; i++)
            {
                var total = trades.Sum(t => t.Periods.FirstOrDefault(p => p.Period == i)?.Volume ?? 0);
                
                yield return new PowerPeriod() { Period = i, Volume = total };
            }
        }
    }
}