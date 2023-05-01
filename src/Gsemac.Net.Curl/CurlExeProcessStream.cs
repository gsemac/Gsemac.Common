using Gsemac.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Curl {

    public class CurlExeProcessStream :
        ProcessStream {

        // Public members

        public CurlExeProcessStream(string filename, IProcessStreamOptions options = null) :
            base(filename, options ?? ProcessStreamOptions.Default) { }
        public CurlExeProcessStream(string filename, string arguments, IProcessStreamOptions options = null) :
            base(filename, arguments, options ?? ProcessStreamOptions.Default) { }
        public CurlExeProcessStream(ProcessStartInfo startInfo, IProcessStreamOptions options = null) :
            base(startInfo, options ?? ProcessStreamOptions.Default) { }

        public override void Close() {

            if (!closed) {

                closed = true;

                // Abort the process and wait for all reads to finish.

                base.Close();

                // Check if curl exited with an error, and throw an exception if so.

                CurlException ex = GetException();

                if (ex is object)
                    throw ex;

            }

        }

        // Protected members

        /// <summary>
        /// Returns a <see cref="CurlException"/> if curl exited with error, or null if it exited without error.
        /// </summary>
        /// <returns>A <see cref="CurlException"/> if curl exited with error, or null if it exited without error.</returns>
        protected CurlException GetException() {

            if (!ProcessExited)
                return null;

            CurlException result = null;

            try {

                int exitCode = ExitCode;

                if (exitCode == -1) {

                    result = new CurlException(exitCode);

                }
                else if (exitCode != 0) {

                    // Curl exited with error; attempt to read the error stream.

                    string stdErrOutput = string.Empty;

                    try {

                        using (StreamReader sr = new StreamReader(StandardError))
                            stdErrOutput = sr.ReadToEnd();

                    }
                    catch (Exception) {
                        // Stream manually disposed by user?
                    }

                    // Parse error message.

                    Match errorMessageMatch = Regex.Match(stdErrOutput, @"curl:\s*\((\d+)\)\s*(.+?)$", RegexOptions.Multiline);

                    if (errorMessageMatch.Success)
                        result = new CurlException(exitCode, errorMessageMatch.Groups[2].Value.Trim());
                    else
                        result = new CurlException(exitCode);

                }

            }
            catch (Exception) {
                // Stream manually disposed by user?
            }

            return result;

        }

        // Private members

        private bool closed = false;

    }

}