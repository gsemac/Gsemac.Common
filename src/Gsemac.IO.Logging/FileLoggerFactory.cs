using System;

namespace Gsemac.IO.Logging {

    public sealed class FileLoggerFactory :
        LoggerFactoryBase {

        // Public members

        public FileLoggerFactory() :
            this(LoggerOptions.Default) {
        }
        public FileLoggerFactory(ILoggerOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        public override ILogger Create() {

            return new FileLogger(options);

        }
        public override ILogger Create(string name) {

            name = PathUtilities.SanitizePath(name, new SanitizePathOptions() {
                StripInvalidFileNameChars = true,
            });

            return new FileLogger(new LoggerOptions(options) {
                FileNameFormatter = new UnixTimestampLogFileNameFormatter(name),
            });

        }

        // Private members

        private readonly ILoggerOptions options;

    }

}