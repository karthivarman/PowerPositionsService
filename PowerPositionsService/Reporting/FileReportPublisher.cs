using Microsoft.Extensions.Options;
using PowerPositionsService.Models;

namespace PowerPositionsService.Reporting
{
    public interface IReportPublisher
    {
        void Publish(string content);
    }

    public class FileReportPublisher : IReportPublisher
    {
        private readonly IOptions<PowerPositionsSettings> _options;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<FileReportPublisher> _logger;

        public FileReportPublisher(IOptions<PowerPositionsSettings> options, IDateTimeService dateTimeService, ILogger<FileReportPublisher> logger)
        {
            _options = options;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public void Publish(string content)
        {
            var now = _dateTimeService.Now;
            var reportFolder = _options.Value.ReportFolder;
            var reportFileName = $"PowerReport_{now:yyyyMMdd}_{now:HHmm}.csv";

            EnsureFolderExists(reportFolder);

            var filePath = Path.Join(reportFolder, reportFileName);
            _logger.LogInformation($"Writing report: {filePath}");

            File.WriteAllText(filePath, content);
        }

        private void EnsureFolderExists(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}
