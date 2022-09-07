namespace Gsemac.IO.Logging {

    public abstract class LoggerFactoryBase :
        ILoggerFactory {

        // Public members

        public abstract ILogger Create();
        public virtual ILogger Create(string name) {

            return new NamedLogger(Create(), name);

        }

    }

}