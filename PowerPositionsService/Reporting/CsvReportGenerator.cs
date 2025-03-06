using System.Text;
using PowerPositionsService.Models;

namespace PowerPositionsService.Reporting
{
    public interface IReportGenerator
    {
        void GenerateRport(IEnumerable<PowerTime> powerTimes);
    }

    public class CsvReportGenerator : IReportGenerator
    {
        private readonly IReportPublisher _publisher;

        public CsvReportGenerator(IReportPublisher publisher)
        {
            _publisher = publisher;
        }

        public void GenerateRport(IEnumerable<PowerTime> powerTimes)
        {
            var csvString = new StringBuilder();

            csvString.AppendLine("LocalTime,Volume");
            foreach (var powerTime in powerTimes)
            {
                csvString.AppendLine($"{powerTime.Time:hh':'mm},{powerTime.Volume}");
            }

            _publisher.Publish(csvString.ToString());
        }
    }
}
