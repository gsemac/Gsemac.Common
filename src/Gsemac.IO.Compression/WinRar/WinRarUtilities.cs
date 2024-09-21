using System;
using System.IO;

namespace Gsemac.IO.Compression.WinRar {

    internal static class WinRarUtilities {

        // Public members

        public static string WinRarExecutableFileName => "Rar.exe";
        public static string UnRarExecutableFileName => "UnRAR.exe";

        public static string GetWinRarExecutablePath(string directoryPath) {

            if (!string.IsNullOrWhiteSpace(directoryPath)) {

                // Attempt to find either command-line executable inside of the given directory.

                string executableFilePath = Path.Combine(directoryPath, WinRarExecutableFileName);

                if (File.Exists(executableFilePath))
                    return executableFilePath;

                executableFilePath = Path.Combine(directoryPath, UnRarExecutableFileName);

                if (File.Exists(executableFilePath))
                    return executableFilePath;

            }
            else {

                return winRarExecutablePath.Value;

            }

            // We were unable to find the executable.

            return string.Empty;

        }

        // Private members

        private static readonly Lazy<string> winRarExecutablePath = new Lazy<string>(GetWinRarExecutablePathInternal);

        private static string GetWinRarExecutablePathInternal() {

            string winrarExecutablePath = ArchiveFactoryUtilities.GetProgramExecutablePath("WinRAR", WinRarExecutableFileName);

            if (string.IsNullOrWhiteSpace(winrarExecutablePath))
                winrarExecutablePath = ArchiveFactoryUtilities.GetProgramExecutablePath("WinRAR", UnRarExecutableFileName);

            return winrarExecutablePath;

        }

    }

}