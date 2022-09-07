using Gsemac.IO.Logging.Properties;
using System;

namespace Gsemac.IO.Logging {

    public sealed class NullLogger :
         ILogger {

        // Public members

        public event LogEventHandler MessageLogged { add { } remove { } }

        public bool Enabled { get; set; }
        public string Name { get; }
        public ILogRetentionPolicy RetentionPolicy {
            get => throw new NotSupportedException(string.Format(ExceptionMessages.ClassDoesNotSupportRetentionPolicies, nameof(NamedLogger)));
            set => throw new NotSupportedException(string.Format(ExceptionMessages.ClassDoesNotSupportRetentionPolicies, nameof(NamedLogger)));
        }

        public void Log(ILogMessage message) { }

    }

}