using PowerPositionsService.Models;
using Services;

namespace PowerPositionsService
{
    public interface IPowerPeriodToPowerTimeConverter
    {
        PowerTime Convert(PowerPeriod powerPeriod, DateTime tradeDay);
    }

    public class PowerPeriodToPowerTimeConverter : IPowerPeriodToPowerTimeConverter
    {
        public PowerTime Convert(PowerPeriod powerPeriod, DateTime tradeDay)
        {
            return new PowerTime
            {
                Time = tradeDay.Date.AddHours(powerPeriod.Period - 2).TimeOfDay,
                Volume = powerPeriod.Volume,
            };
        }
    }
}
