using System;

namespace Gsemac.Net.Curl {

    public class BinCurlException :
        Exception {

        /// <summary>
        /// Returns the exit code the process exited with.
        /// </summary>
        public int ExitCode { get; }

        public BinCurlException(int exitCode) :
           base(((CurlCode)exitCode).ToString()) {

            ExitCode = exitCode;

        }
        public BinCurlException(int exitCode, string message) :
            base(message) {

            ExitCode = exitCode;

        }

    }

}