using System;

namespace Gsemac.IO.Logging {

    public class FileLogger :
        SynchronizedLoggerBase,
        IFileLogger {

        // Public members

        public string Directory { get; set; }
        public ILogFilenameFormatter FilenameFormatter { get; set; } = new LogFilenameFormatter();

        public FileLogger() {
        }
        public FileLogger(string directory) {

            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentNullException(nameof(directory));

            Directory = directory;

        }

        public void SetLogRetentionPolicy(ILogRetentionPolicy retentionPolicy) {

            logRetentionPolicy = retentionPolicy;

            ExecuteLogRetentionPolicy();

        }

        // Protected members

        protected override void Log(ILogMessage logMessage, string formattedMessage) {

            CreateLogDirectory();

            CreateLogFile();

            ExecuteLogRetentionPolicy();

            string logFilePath = currentFilename;

            if (!string.IsNullOrWhiteSpace(Directory))
                logFilePath = System.IO.Path.Combine(Directory, logFilePath);

            System.IO.File.AppendAllText(logFilePath, formattedMessage);

        }

        // Private members

        private string currentFilename = string.Empty;
        private ILogRetentionPolicy logRetentionPolicy = new NeverDeleteLogRetentionPolicy();

        private void CreateLogDirectory() {

            if (!string.IsNullOrWhiteSpace(Directory) && !System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);

        }
        private void CreateLogFile() {

            if (string.IsNullOrWhiteSpace(currentFilename))
                currentFilename = FilenameFormatter.Format(DateTime.Now);

        }
        private void ExecuteLogRetentionPolicy() {

            try {

                if (Enabled && logRetentionPolicy != null)
                    logRetentionPolicy.ExecutePolicy(Directory, $"*{FilenameFormatter.FileExtension}");

            }
            catch (Exception ex) {

                if (!IgnoreExceptions)
                    throw ex;

            }

        }

    }

}