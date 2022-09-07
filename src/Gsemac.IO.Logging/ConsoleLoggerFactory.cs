using System;

namespace Gsemac.IO.Logging {

    public sealed class ConsoleLoggerFactory :
        LoggerFactoryBase {

        // Public members

        public ConsoleLoggerFactory() :
            this(LoggerOptions.Default) {
        }
        public ConsoleLoggerFactory(ILoggerOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        public override ILogger Create() {

            return new ConsoleLogger(options);

        }

        // Private members

        private readonly ILoggerOptions options;

    }

}