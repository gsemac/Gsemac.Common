namespace Gsemac.IO.Logging {

    public abstract class SynchronizedLoggerBase :
        LoggerBase {

        // Public members

        public sealed override void Log(ILogMessage message) {

            lock (mutex) {

                base.Log(message);

            }

        }

        // Protected members

        public SynchronizedLoggerBase() :
            this(LoggerOptions.Default) {
        }
        public SynchronizedLoggerBase(ILoggerOptions options) :
            base(options) {
        }

        // Private members

        private readonly object mutex = new object();

    }

}