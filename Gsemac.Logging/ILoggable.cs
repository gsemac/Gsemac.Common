using System;

namespace Gsemac.Logging {

    public interface ILoggable {

        event EventHandler<LogEventArgs> Log;

    }

}