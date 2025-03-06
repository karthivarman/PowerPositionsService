using PowerPositionsService.Models;
using PowerPositionsService.Reporting;
using Serilog;
using Services;

namespace PowerPositionsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configurationSection = configurationBuilder.Build().GetSection("PowerPositionSettings");

            // Logging
            var logPath = configurationSection["LogPath"];
            if (string.IsNullOrEmpty(logPath))
            {
                throw new Exception($"Invalid log path: {logPath}");
            }

            var logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Logger = logger;


            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSerilog();

                    // Dependencies
                    services.AddTransient<IPowerService, PowerService>();
                    services.AddTransient<IPowerServiceWrapper, PowerServiceWrapper>();
                    services.AddTransient<IPowerServiceProcessor, PowerServiceProcessor>();
                    services.AddTransient<ITradesAggregator, HourlyTradesAggregator>();
                    services.AddTransient<IPowerPeriodToPowerTimeConverter, PowerPeriodToPowerTimeConverter>();
                    services.AddTransient<IReportGenerator, CsvReportGenerator>();
                    services.AddTransient<IReportPublisher, FileReportPublisher>();
                    services.AddTransient<IDateTimeService, DateTimeService>();

                    services.Configure<PowerPositionsSettings>(configurationSection);

                    services.AddHostedService<Worker>();
                });

            var host = builder.Build();
            host.Run();
        }
    }
}