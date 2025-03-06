using PowerPositionsService;
using Services;

namespace PowerPositionService.Tests
{
    public class HourlyTradesAggregatorTests
    {
        HourlyTradesAggregator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new HourlyTradesAggregator();
        }

        [Test]
        public void Aggregate_TwoPositiveVolumeInputs_ReturnAggregateVolumes()
        {
            // Arrange
            var tradeDate = new DateTime(2025, 3, 5);

            var powerTrade1 = PowerTrade.Create(tradeDate, 24);
            var powerTrade2 = PowerTrade.Create(tradeDate, 24);

            for(var i = 1; i<= 24; i++)
            {
                powerTrade1.Periods.First(p => p.Period == i).Volume = 10;
                powerTrade2.Periods.First(p => p.Period == i).Volume = 5;
            }

            // Act
            var result = _sut.Aggregate(new[] { powerTrade1, powerTrade2 });

            // Assert
            for(var period = 1; period <= 24; period++)
            {
                Assert.That(result.Single(r => r.Period == period).Volume, Is.EqualTo(15));
            }
        }

        [Test]
        public void Aggregate_PositiveAndNegativeVolumeInputs_ReturnAggregateVolumes()
        {
            // Arrange
            var tradeDate = new DateTime(2025, 3, 5);

            var powerTrade1 = PowerTrade.Create(tradeDate, 24);
            var powerTrade2 = PowerTrade.Create(tradeDate, 24);

            for (var i = 1; i <= 24; i++)
            {
                powerTrade1.Periods.First(p => p.Period == i).Volume = 10;
                powerTrade2.Periods.First(p => p.Period == i).Volume = -5;
            }

            // Act
            var result = _sut.Aggregate(new[] { powerTrade1, powerTrade2 });

            // Assert
            for (var period = 1; period <= 24; period++)
            {
                Assert.That(result.Single(r => r.Period == period).Volume, Is.EqualTo(5));
            }
        }

        [Test]
        public void Aggregate_ZeroVolumeInputs_ReturnAggregateVolumes()
        {
            // Arrange
            var tradeDate = new DateTime(2025, 3, 5);

            var powerTrade1 = PowerTrade.Create(tradeDate, 24);
            var powerTrade2 = PowerTrade.Create(tradeDate, 24);

            for (var i = 1; i <= 24; i++)
            {
                powerTrade1.Periods.First(p => p.Period == i).Volume = 0;
                powerTrade2.Periods.First(p => p.Period == i).Volume = 0;
            }

            // Act
            var result = _sut.Aggregate(new[] { powerTrade1, powerTrade2 });

            // Assert
            for (var period = 1; period <= 24; period++)
            {
                Assert.That(result.Single(r => r.Period == period).Volume, Is.EqualTo(0));
            }
        }

        [Test]
        public void Aggregate_EmptuInputs_ReturnZeroVolumes()
        {
            // Arrange
            var tradeDate = new DateTime(2025, 3, 5);

            // Act
            var result = _sut.Aggregate(new PowerTrade[0]).ToList();

            // Assert
            for (var period = 1; period <= 24; period++)
            {
                Assert.That(result.Single(r => r.Period == period).Volume, Is.EqualTo(0));
            }
        }
    }
}
