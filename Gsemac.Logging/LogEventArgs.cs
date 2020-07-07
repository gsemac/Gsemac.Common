using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Logging {

    public class LogEventArgs :
        EventArgs {

        // Public members

        public ILogMessage Message { get; }

        public LogEventArgs(ILogMessage logMessage) {

            this.Message = logMessage;

        }

        public override string ToString() {

            return Message.ToString();

        }

    }

}