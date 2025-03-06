namespace PowerPositionsService.Models
{
    public class PowerPositionsSettings
    {
        public string LogPath { get; set; }

        public string ReportFolder { get; set; }

        public int IntervalInSeconds { get; set; }

        public int MaxRetryCount { get; set; }
    }
}