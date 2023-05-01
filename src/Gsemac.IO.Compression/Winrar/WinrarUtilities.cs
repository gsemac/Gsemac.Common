using System;
using System.IO;

namespace Gsemac.IO.Compression.Winrar {

    internal static class WinrarUtilities {

        // Public members

        public static string WinrarExecutableFileName => "Rar.exe";
        public static string UnrarExecutableFileName => "UnRAR.exe";

        public static string GetWinrarExecutablePath(string directoryPath) {

            if (!string.IsNullOrWhiteSpace(directoryPath)) {

                // Attempt to find either command-line executable inside of the given directory.

                string executableFilePath = Path.Combine(directoryPath, WinrarExecutableFileName);

                if (File.Exists(executableFilePath))
                    return executableFilePath;

                executableFilePath = Path.Combine(directoryPath, UnrarExecutableFileName);

                if (File.Exists(executableFilePath))
                    return executableFilePath;

            }
            else {

                return winrarExecutablePath.Value;

            }

            // We were unable to find the executable.

            return string.Empty;

        }

        // Private members

        private static readonly Lazy<string> winrarExecutablePath = new Lazy<string>(GetWinrarExecutablePathInternal);

        private static string GetWinrarExecutablePathInternal() {

            string winrarExecutablePath = ArchiveFactoryUtilities.GetProgramExecutablePath("WinRAR", WinrarExecutableFileName);

            if (string.IsNullOrWhiteSpace(winrarExecutablePath))
                winrarExecutablePath = ArchiveFactoryUtilities.GetProgramExecutablePath("WinRAR", UnrarExecutableFileName);

            return winrarExecutablePath;

        }

    }

}