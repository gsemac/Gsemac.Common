using System;
using System.Diagnostics;

namespace Gsemac.Polyfills.System.Diagnostics {

    public static class ProcessExtensions {

        // Public members

        /// <summary>
        /// Instructs the Process component to wait the specified amount of time for the associated process to exit.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="timeout">The amount of time to wait for the associated process to exit.</param>
        /// <returns>true if the associated process has exited; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool WaitForExit(this Process process, TimeSpan timeout) {

            if (process is null)
                throw new ArgumentNullException(nameof(process));

            return process.WaitForExit((int)timeout.TotalMilliseconds);

        } // .NET 7 and later

    }

}