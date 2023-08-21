using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Gsemac.Core {

    public static class ProcessUtilities {

        // Public members

        public static IEnumerable<Process> GetProcessesByFileName(string fileName) {

            // A process' "ProcessName" property is simply the executable file name without the file extension.
            // https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.processname?view=net-7.0
            // "GetProcessesByName" can therefore be used with a file name reliably.

            if (string.IsNullOrWhiteSpace(fileName))
                return Enumerable.Empty<Process>();

            string processName = Path.GetFileNameWithoutExtension(fileName);

            if (string.IsNullOrEmpty(processName))
                return Enumerable.Empty<Process>();

            return Process.GetProcessesByName(processName);

        }
        public static IEnumerable<Process> GetProcessesByFilePath(string filePath) {

            return GetProcessesByFileName(filePath)
                .Where(process => ProcessHasMatchingFileName(process, filePath));

        }

        // Private members

        private static bool TryGetProcessFilePath(Process process, out string filePath) {

            filePath = null;

            if (process is null)
                throw new ArgumentNullException(nameof(process));

            try {

                // The "Filename" property will return a fully-qualified file name.
                // Note that accessing the "MainModule" property can throw an exception under certain conditions.

                string processFilePath = process.MainModule.FileName;

                // The path is normalized to ease comparisons.

                if (!string.IsNullOrWhiteSpace(processFilePath))
                    processFilePath = NormalizeProcessFilePath(processFilePath);

                filePath = processFilePath;

                return true;

            }
            catch {

                return false;

            }

        }
        private static string NormalizeProcessFilePath(string processPath) {

            if (string.IsNullOrWhiteSpace(processPath))
                return processPath;

            return Path.GetFullPath(processPath)
                .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

        }
        private static bool ProcessHasMatchingFileName(Process process, string filePath) {

            if (process is null)
                throw new ArgumentNullException(nameof(process));

            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            // Directory separators are normalized before doing the comparison.

            return TryGetProcessFilePath(process, out string processFilePath) &&
                processFilePath.Equals(NormalizeProcessFilePath(filePath));

        }

    }

}