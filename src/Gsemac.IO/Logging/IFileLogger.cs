namespace Gsemac.IO.Logging {

    public interface IFileLogger :
        ILogger {

        string Directory { get; set; }
        ILogFilenameFormatter FilenameFormatter { get; set; }

        void SetLogRetentionPolicy(ILogRetentionPolicy retentionPolicy);

    }

}