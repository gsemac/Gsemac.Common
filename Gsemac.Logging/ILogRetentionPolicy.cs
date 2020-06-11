using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Logging {

    public interface ILogRetentionPolicy {

        void ExecutePolicy(string directoryPath, string searchPattern = "*");

    }

}