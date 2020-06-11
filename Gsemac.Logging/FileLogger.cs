using System;

namespace Gsemac.Logging {

    public class FileLogger :
        SynchronizedLoggerBase,
        IFileLogger {

        // Public members

        public string Directory { get; set; }
        public ILogFilenameFormatter FilenameFormatter { get; set; } = new TimestampedLogFilenameFormatter();
        public ILogRetentionPolicy RetentionPolicy { get; set; } = new DeleteOldLogRetentionPolicy(TimeSpan.FromDays(3));

        public FileLogger() {
        }
        public FileLogger(string directory) {

            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentNullException(nameof(directory));

            this.Directory = directory;

        }

        // Protected members

        protected override void Log(ILogMessage logMessage, string formattedMessage) {

            CreateDirectoryIfItDoesNotExist();
            InitializeFileIfItIsNotInitialized();

            string logFilePath = currentFilename;

            if (!string.IsNullOrWhiteSpace(Directory))
                logFilePath = System.IO.Path.Combine(Directory, logFilePath);

            System.IO.File.AppendAllText(logFilePath, formattedMessage);

        }

        // Private members

        private string currentFilename = string.Empty;

        private void CreateDirectoryIfItDoesNotExist() {

            if (!string.IsNullOrWhiteSpace(Directory) && !System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);

        }
        private void InitializeFileIfItIsNotInitialized() {

            if (string.IsNullOrWhiteSpace(currentFilename)) {

                currentFilename = FilenameFormatter.Format(DateTime.Now);

                // Execute the log file retention policy when creating a new file.

                try {

                    if (RetentionPolicy != null)
                        RetentionPolicy.ExecutePolicy(Directory, $"*{FilenameFormatter.FileExtension}");

                }
                catch (Exception ex) {

                    if (!IgnoreExceptions)
                        throw ex;

                }

            }

        }

    }

}