using PowerPositionsService;
using Services;

namespace PowerPositionService.Tests
{
    public class PowerPeriodToPowerTimeConverterTests
    {
        PowerPeriodToPowerTimeConverter _sut;
        DateTime _today;

        [SetUp]
        public void Setup()
        {
            _today = new DateTime(2025, 3, 5);

            _sut = new PowerPeriodToPowerTimeConverter();
        }

        [Test]
        [TestCase(1, 23)]
        [TestCase(2, 0)]
        [TestCase(3, 1)]
        [TestCase(4, 2)]
        [TestCase(5, 3)]
        [TestCase(6, 4)]
        [TestCase(7, 5)]
        [TestCase(8, 6)]
        [TestCase(9, 7)]
        [TestCase(10, 8)]
        [TestCase(11, 9)]
        [TestCase(12, 10)]
        [TestCase(13, 11)]
        [TestCase(14, 12)]
        [TestCase(15, 13)]
        [TestCase(16, 14)]
        [TestCase(17, 15)]
        [TestCase(18, 16)]
        [TestCase(19, 17)]
        [TestCase(20, 18)]
        [TestCase(21, 19)]
        [TestCase(22, 20)]
        [TestCase(23, 21)]
        [TestCase(24, 22)]
        public void Convert_ConvertsPeriodToTime(int period, int expectedHour)
        {
            // Arrange
            var powerPeriod = new PowerPeriod() { Period = period, Volume = 10 };

            // Act
            var result = _sut.Convert(powerPeriod, _today);

            // Assert
            Assert.That(result.Time, Is.EqualTo(TimeSpan.FromHours(expectedHour)));
        }
    }
}