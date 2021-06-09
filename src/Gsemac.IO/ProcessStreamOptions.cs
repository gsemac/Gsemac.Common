using System;

namespace Gsemac.IO {

    [Flags]
    public enum ProcessStreamOptions {
        /// <summary>
        /// Redirect Standard Input to the stream.
        /// </summary>
        RedirectStandardOutput = 1,
        /// <summary>
        /// Redirect writes to the stream to Standard Input.
        /// </summary>
        RedirectStandardInput = 2,
        /// <summary>
        /// Redirect Standard Error to the stream.
        /// </summary>
        RedirectStandardError = 4,
        /// <summary>
        /// Redirect Standard Output and Standard Error to the same stream.
        /// </summary>
        RedirectStandardErrorToStandardOutput = 8,
        Default = RedirectStandardOutput | RedirectStandardInput | RedirectStandardError
    }

}