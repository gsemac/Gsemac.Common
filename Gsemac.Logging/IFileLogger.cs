namespace Gsemac.Logging {

    public interface IFileLogger :
        ILogger {

        string Directory { get; set; }
        ILogFilenameFormatter FilenameFormatter { get; set; }
        ILogRetentionPolicy RetentionPolicy { get; set; }

    }

}