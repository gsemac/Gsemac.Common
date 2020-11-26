using Gsemac.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Curl {

    public class BinCurlProcessStream :
        ProcessStream {

        // Public members

        public BinCurlProcessStream(string filename, ProcessStreamOptions options = ProcessStreamOptions.Default) :
            base(filename, options) { }
        public BinCurlProcessStream(string filename, string arguments, ProcessStreamOptions options = ProcessStreamOptions.Default) :
            base(filename, arguments, options) { }
        public BinCurlProcessStream(ProcessStartInfo startInfo, ProcessStreamOptions options = ProcessStreamOptions.Default) :
            base(startInfo, options) { }

        public override void Close() {

            if (closed)
                return;

            closed = true;

            // Abort the process and wait for all reads to finish.
            Abort(true);

            // Check if curl exited with an error, and throw an exception if so.
            BinCurlException ex = GetException();

            // Close the base stream before throwing to ensure it's properly cleaned up first.
            base.Close();

            if (ex != null)
                throw ex;

        }

        // Protected members

        /// <summary>
        /// Returns a <see cref="BinCurlException"/> if curl exited with error, or null if it exited without error.
        /// </summary>
        /// <returns>A <see cref="BinCurlException"/> if curl exited with error, or null if it exited without error.</returns>
        protected BinCurlException GetException() {

            if (!ProcessExited)
                return null;

            BinCurlException result = null;

            try {

                int exitCode = ExitCode;

                if (exitCode == -1) {

                    result = new BinCurlException(exitCode, "Process was terminated prematurely.");

                }
                else if (exitCode != 0) {

                    // Curl exited with error; attempt to read the error stream.

                    string stdErrOutput = string.Empty;

                    try {

                        using (StreamReader sr = new StreamReader(StandardErrorStream))
                            stdErrOutput = sr.ReadToEnd();

                    }
                    catch (Exception) {
                        // Stream manually disposed by user?
                    }

                    // Parse error message.

                    Match errorMessageMatch = Regex.Match(stdErrOutput, @"curl:\s*\((\d+)\)\s*(.+?)$", RegexOptions.Multiline);

                    if (errorMessageMatch.Success)
                        result = new BinCurlException(exitCode, errorMessageMatch.Groups[2].Value.Trim());
                    else
                        result = new BinCurlException(exitCode);

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