using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Logging {

    public abstract class SynchronizedLoggerBase :
        LoggerBase {

        // Public members

        public override void Log(ILogMessage message) {

            lock (lockObject) {

                base.Log(message);

            }

        }

        // Private members

        private readonly object lockObject = new object();

    }

}