﻿using Gsemac.Utilities;
using System;
using System.Text;

namespace Gsemac.Logging {

    public class TimestampedLogFilenameFormatter :
        ILogFilenameFormatter {

        // Public members

        public string Name { get; set; } = "debug";
        public string FileExtension { get; set; } = ".log";

        public string Format(DateTime timestamp) {

            StringBuilder sb = new StringBuilder();

            sb.Append(DateUtilities.ToUnixTimeSeconds(timestamp));

            if (!string.IsNullOrWhiteSpace(Name))
                sb.Append($"-{Name}");

            if (!string.IsNullOrWhiteSpace(FileExtension) && !Name.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase))
                sb.Append(FileExtension);

            return sb.ToString();

        }

    }

}