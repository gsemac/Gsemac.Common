﻿namespace Gsemac.IO.Logging {

    public abstract class SynchronizedLoggerBase :
        LoggerBase {

        // Public members

        public override void Log(ILogMessage message) {

            lock (lockObject) {

                base.Log(message);

            }

        }

        // Protected members

        public SynchronizedLoggerBase() {
        }
        public SynchronizedLoggerBase(ILoggerOptions options) :
            base(options) {
        }
        public SynchronizedLoggerBase(bool enabled) :
            base(enabled) {
        }
        public SynchronizedLoggerBase(bool enabled, ILoggerOptions options) :
          base(enabled, options) {
        }

        // Private members

        private readonly object lockObject = new object();

    }

}