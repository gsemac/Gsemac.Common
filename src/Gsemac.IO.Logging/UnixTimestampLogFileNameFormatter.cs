using Gsemac.Core;
using System;
using System.Text;

namespace Gsemac.IO.Logging {

    public class UnixTimestampLogFileNameFormatter :
        LogFileNameFormatterBase {

        // Public members

        public UnixTimestampLogFileNameFormatter() {
        }
        public UnixTimestampLogFileNameFormatter(string name) :
            base(name) {
        }

        public override string GetFileName() {

            StringBuilder sb = new StringBuilder();

            sb.Append(DateUtilities.ToUnixTimeSeconds(DateTime.Now));

            string baseFileName = base.GetFileName();

            if (!string.IsNullOrWhiteSpace(baseFileName))
                sb.Append($"-{baseFileName}");

            return sb.ToString();

        }

    }

}