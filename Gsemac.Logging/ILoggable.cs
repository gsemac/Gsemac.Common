using System;

namespace Gsemac.Logging {

    public interface ILoggable {

        event LogEventHandler Log;

    }

}